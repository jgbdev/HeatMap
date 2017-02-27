#RestfulClient.py

import requests
from subprocess import check_output
from threading import Timer,Thread,Event


class perpetualTimer():

   def __init__(self,t,hFunction):
      self.t=t
      self.hFunction = hFunction
      self.thread = Timer(self.t,self.handle_function)

   def handle_function(self):
      self.hFunction()
      self.thread = Timer(self.t,self.handle_function)
      self.thread.start()

   def start(self):
      self.thread.start()

   def cancel(self):
      self.thread.cancel()
      

# Replace with the correct URL
url = "http://34.251.68.107:5000/api/"
id = "ade79c00-dc89-4158-8a8b-b41d2a3d885c" # Set to None to get new id
interval = 0
mode = "rpi" # "rpi" or "sensors"

def GET(api):
    response = requests.get(url + api, verify=True)
    if (not response.ok):
        print("GET request failed!")
        response.raise_for_status()
        return None
    else:
        return response.json()
    
def POST(api, payload):
    response = requests.post(url + api, data=payload, verify=True)
    if (not response.ok):
        print("POST request failed!")
        response.raise_for_status()
        return None
    else:
        jData = response.json()
        return jData

def GetID():
    result = POST("device", {})
    return result['device_id']

def GetInterval():
    return GET("device/" + id)['refresh_time']
    
def SendReading():
    payload = None
    if (mode == "rpi"):
        print "Using 'rpi' mode"
        tempStr = check_output(["/opt/vc/bin/vcgencmd", "measure_temp"])
        temp = tempStr.split("=")[1].split("'")[0]
        temp = float(temp)
        print temp
        payload = { "data": [{ "hardware_id": id, "sensor_info": [{ "tag": "cpu_temperature", "value": temp }] }] }
        print payload
        print "Uploading reading"
        POST("reading/" + id, payload)
    elif (mode == "sensors"):
        print "Using 'sensors' mode"
        
    else:
        print "Unrecognised mode!"

    
if id == None:
    id = GetID()
    print "New Id: " + id
else:
    print "Existing Id: " + id

interval = GetInterval()
print interval

SendReading()

t = perpetualTimer(int(interval/1000),SendReading)
t.start()

while True:
    user_input = raw_input("Enter 'quit' to quit...:")
    if user_input == "quit":
        t.cancel()
        break
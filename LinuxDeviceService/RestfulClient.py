# -*- coding: latin-1 -*-
#RestfulClient.py

import requests
import json
from subprocess import check_output
from threading import Timer,Thread,Event
import os.path


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
      
def GET(api):
    response = requests.get(url + api, verify=True)
    if (not response.ok):
        print("GET request failed!")
        response.raise_for_status()
        return None
    else:
        return response.json()
    
def POST(api, payload):
    xdata = json.dumps(payload)
    headers = {'Content-type': 'application/json', 'Accept': 'application/json'}
    response = requests.post(url + api, data=xdata, headers=headers, verify=True)
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
        payload = { "data": [{ "hardware_id": "cpu", "sensor_info": [{ "tag": "cpu_temperature", "value": temp }] }] }
        print payload
        print "Uploading reading"
        POST("reading/" + id, payload)
    elif (mode == "sensors"):
        print "Using 'sensors' mode"
        
        #sampleData = "acpitz-virtual-0\nAdapter: Virtual device\ntemp1:        +80.0°C  (crit = +106.0°C)\ntemp2:        +29.8°C  (crit = +106.0°C)\n\ncoretemp-isa-0000\nAdapter: ISA adapter\nPhysical id 0:  +81.0°C  (high = +87.0°C, crit = +105.0°C)\nCore 0:         +74.0°C  (high = +87.0°C, crit = +105.0°C)\nCore 1:         +79.0°C  (high = +87.0°C, crit = +105.0°C)\nCore 2:         +81.0°C  (high = +87.0°C, crit = +105.0°C)\nCore 3:         +78.0°C  (high = +87.0°C, crit = +105.0°C)"
        
        sampleData = check_output(["sensors"])
        
        lines = sampleData.split('\n')
        temperatures = []
        for line in lines:
            stripLine = line.strip()
            if (stripLine.startswith("temp")):
                tempPart = stripLine.split('+')[1]
                tempPart = tempPart.split('°')[0]
                temp = float(tempPart)
                temperatures.append(temp)
            elif (stripLine.startswith("Core ")):
                tempPart = stripLine.split('+')[1]
                tempPart = tempPart.split('°')[0]
                temp = float(tempPart)
                temperatures.append(temp)
        
        sensor_info = []
        for temp in temperatures:
            sensor_info.append({ "tag": "cpu_temperature", "value": temp })
        
        payload = { "data": [{ "hardware_id": "cpu", "sensor_info": sensor_info }] }
        print payload
        print "Uploading reading"
        POST("reading/" + id, payload)
    else:
        print "Unrecognised mode!"



# Replace with the correct URL
url = "http://34.251.68.107:5000/api/"
id = "ade79c00-dc89-4158-8a8b-b41d2a3d885c" # Set to None to get new id
interval = 0
mode = "sensors" # "rpi" or "sensors"

configValid = False
configFile = None
if (os.path.isfile("heatmap_config.txt")):
    configFile = open("heatmap_config.txt", "r")
    lines = configFile.readlines()
    configFile.close()
    if (len(lines) >= 4):
        configValid = True
        url = lines[0].strip()
        id = lines[3].strip()
        if (len(lines) >= 6):
            mode = lines[5].strip()
        else:
            mode = raw_input("What mode should this execute in? sensors or rpi:").strip()
        
        print "Existing Id: " + id
        
if (not configValid):
    id = GetID()
    print "New Id: " + id
    
    configFile = open("heatmap_config.txt", "w")
    configFile.write(url + "\n")
    configFile.write("\n")
    configFile.write("\n")
    configFile.write(id + "\n")
    configFile.write("False\n")
    mode = raw_input("What mode should this execute in? sensors or rpi:")
    configFile.write(mode + "\n")
    
configFile.close()

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
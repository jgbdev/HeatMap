#RestfulClient.py

import requests
from subprocess import check_output

# Replace with the correct URL
url = "http://34.251.68.107:5000/api/"
id = "ade79c00-dc89-4158-8a8b-b41d2a3d885c"
interval = 30000
mode = "rpi" # or "sensors"

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
    
if id == None:
    id = GetID()
    print "New Id: " + id
else:
    print "Existing Id: " + id

interval = GetInterval()
print interval

payload = None
if (mode == "rpi"):
    print "Using 'rpi' mode"
    tempStr = check_output(["/opt/vc/bin/vcgencmd", "measure_temp"])
    temp = tempStr.split("=")[1].split("'")[0]
    payload = '{ data: [{ "hardware_id": "' + id + '", "sensor_info": [{ "tag": "cpu_temperature", "value": ' + temp + ' }] }]'
elif (mode == "sensors"):
    print "Using 'sensors' mode"
    
else:
    print "Unrecognised mode!"

# print "XYZ"
# result = call(["ls", "-l"])
# print result
# -*- coding: latin-1 -*-
#RestfulClient.py

import requests
import json
from subprocess import check_output
from threading import Timer,Thread,Event
import os.path
      
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

def GetReadings(id):
    result = GET("reading/" + id + "/from/0")
    return result

# Replace with the correct URL
url = "http://34.251.68.107:5000/api/"
id = None

deviceIdData = GET("device")
for val in deviceIdData:
    print val['device_id']

user_input = raw_input("Enter device id to analyse:")
id = user_input.strip()
print "Analysing: " + id

readingData = GetReadings(id)
#print readingData ##BAD IDEA!

readingData = readingData['data']
temp_min = 999999999
temp_max = -999999999
temp_avg = 0.0
temp_count = 0.0
load_min = 999999999
load_max = -999999999
load_avg = 0.0
load_count = 0.0
for data in readingData:
    #print data ##Not quite such a bad idea
    if ('data' in data):
        hwinfos = data['data']
        for hwinfo in hwinfos:
            if (type(hwinfo) is dict and 'sensor_info' in hwinfo):
                sensorinfos = hwinfo['sensor_info']
                for sensorinfo in sensorinfos:
                    val = sensorinfo['value']
                    if sensorinfo['tag'].find("temp") > -1:
                        # Temperature
                        if (val < temp_min):
                            temp_min = val
                        if (val > temp_max):
                            temp_max = val
                        temp_avg = (temp_avg * (temp_count / (temp_count+1))) + (val / (temp_count+1))
                        temp_count = temp_count + 1
                    elif sensorinfo['tag'].find("load") > -1:
                        # Load
                        if (val < load_min):
                            load_min = val
                        if (val > load_max):
                            load_max = val
                        load_avg = (load_avg * (load_count / (load_count+1))) + (val / (load_count+1))
                        load_count = load_count + 1
            
print "Temp (" + str(temp_count) + "): Min: " + str(temp_min) + ", Avg: " + str(temp_avg) + ", Max: " + str(temp_max)
print "Load (" + str(load_count) + "): Min: " + str(load_min) + ", Avg: " + str(load_avg) + ", Max: " + str(load_max)
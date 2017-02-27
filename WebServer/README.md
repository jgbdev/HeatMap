### API



## Device

[POST] /api/device
Body: {
    coordinates : {
        lat: <float>
        long: <float>
    }
}


## Register hardware

[POST] `/api/hardware/:device_id`

BODY

```
{
  "name": <string>,
  "type": <string>
}                    
```

RESPONSE:

```
{
    "hardware_id" : <uint>
}
```


## Get Device Info

[GET] `/api/device/:id`

RESPONSE

BODY
```
{
    device_id: <uint>,
    coordinates : {
        lat: <float>
        long: <float>
    },
    refresh_time: <uint>,   (Miliseconds)
    hardware_ids : [ <uint> ]
}
```

## Send reading

[POST] `/api/reading/:device_id`

BODY
```
{
    data : [{
        "hardware_id" : <string>,
        "device_info" : [
            {<tag> : <float_value> }
        ]
    }]
}
```

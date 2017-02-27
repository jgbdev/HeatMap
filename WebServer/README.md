### API



### Devices


#Request a new device ID

[POST] `/api/device`

BODY

```
{
    coordinates : {
        lat: <float>
        long: <float>
    }
}
```

RESPONSE
```
{
    <device_id> : <uint>
}

```


## Get all Devices

[GET] `/api/device`


RESPONSE

```
{
    "devices" : [<string>]
}

```


## Get Device Info

[GET] `/api/device/:id`

RESPONSE

BODY
```
{
    device_id: <string> ,
    coordinates : {
        lat: <float>
        long: <float>
    },
    refresh_time: <uint>   (Miliseconds)

}
```





## List Devices



## Send reading

[POST] `/api/reading/:device_id`

BODY
```
{
    data : [{
        "hardware_id" : <string>,
        "sensor_info" : [
            {
                "tag" : <string>,
                "value": <float>
            }
        ]
    }]
}
```

## Get data


[GET] `/api/reading/:device_id`

Retrieves the latest data

BODY
```
{
    data : [{
        "hardware_id" : <string>,
        "sensor_info" : [
            {
                "tag" : <string>,
                "value": <float>
            }
        ]
    }]
}
```




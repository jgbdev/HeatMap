### API



### Devices


#Request a new device ID

[POST] `/api/device`

BODY

```
{
    "coordinates" : {
        "lat": <float>
        "long": <float>
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


## Update Device Information

[PATCH] `/api/device/:id`

BODY
```
{
    "coordinates" : {
        "lat": <float>
        "long": <float>
    }
}

```


RESPONSE

```
{
    "devices" : [<string>]
}

```



## Get Latest Device Info

[GET] `/api/device/:id`

RESPONSE

BODY
```
{
    "device_id": <string> ,
    "coordinates" : {
        "lat": <float>
        "long": <float>
    },
    "refresh_time": <uint>   (Miliseconds)

}
```




## List Devices



## Send reading

[POST] `/api/reading/:device_id`

BODY
```
{
    "data" : [{
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

## Reading data from a unix timestamp


[GET] `/api/reading/:device_id`


BODY
```
{
    "data" : [{
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


## Reading data between two unix timestamps

[GET] `/api/reading/:device_id/from/:from/to/:to`



BODY
```
{

    "data" : [
        {
            "time_stamp" : <uint>,
            "data" : [{
                    "hardware_id" : <string>,
                    "sensor_info" : [
                        {
                            "tag" : <string>,
                            "value": <float>
                        }
                    ]
                }]
        }
    ]
}
```





[GET] `/api/reading/:device_id/from/:from/to/:to`

Retrieves the latest data

BODY
```
{

    "data" : [
        {
            "time_stamp" : <uint>,
            "data" : [{
                    "hardware_id" : <string>,
                    "sensor_info" : [
                        {
                            "tag" : <string>,
                            "value": <float>
                        }
                    ]
                }]
        }
    ]
}
```

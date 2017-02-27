### API



## Device

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
    refresh_time: <uint>   (Miliseconds)

}
```

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

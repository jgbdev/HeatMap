### API



## Device

[POST] /api/device
Body: {

    device_id: <uint>,
    coordinates : {
        lat: <float>
        long: <float>
    }
    refresh_time: <uint>   (Miliseconds)
}


[POST] /api/hardware/:device_id
Body: {

}


[GET] /api/device/:id
Resp:
BODY
{
    device_id: <uint>,
    coordinates : {
        lat: <float>
        long: <float>
    }
    refresh_time: <uint>   (Miliseconds)
}

device id, hardware id  tag float
POST /api/reading/:device_id
BODY
{
    time: <unix_time>
    data : [{
        "hardware_id" : <uint>,
        "device_info" : [
            {<tag> : <float_value> }
        ]
    }]
}



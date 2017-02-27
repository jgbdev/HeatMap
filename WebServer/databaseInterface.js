"use strict";
module.exports =  class DatabaseInterface {

    constructor(){
        this.device_id = 0;
    }


    device(req, res){
        if(req.method == "POST"){
            res.json({
                "device_id" : this.device_id ++
            });
        }else if(req.method == "GET"){
            let id = req.params.id;
            res.json({
                    device_id: id,
                    coordinates : {
                    lat: 0.11,
                    long: 0.12
                },
                refresh_time: 30000
            });
        }
    }

    reading(req, res){
        if(req.method == "POST"){
            let body = req.body;
            console.log(body);
            res.json({});
        }else if(req.method == "GET"){

        }
    }

};


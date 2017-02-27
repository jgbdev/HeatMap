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
                refresh_time: 20
            });
        }
    }

    reading(req, res){
        if(req.method == "POST"){


        }else if(req.method == "GET"){

        }
    }

};


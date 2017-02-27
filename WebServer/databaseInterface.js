"use strict";
let r = require('rethinkdb');

let connection = null;
let db_name = 'test';
let refresh_time = 30000;
module.exports =  class DatabaseInterface {

    constructor(){
        this.connect(this);
    }


    connect(me){

        r.connect({
            host: 'localhost',
            port: '32769',
            db : db_name
        }, function (err, conn){
            console.log("Connected");
            connection = conn;
            me.setupTables();
        });

    }

    setupTables(){

        r.tableCreate('devices',  {primaryKey: 'device_id'}).run(connection, function(err, conn){
            if(err){
                console.log(err);
            }
        });


        r.tableCreate('readings',  {primaryKey: 'id'}).run(connection, function(err, conn){
            if(err){
                console.log(err);
            }
        });
    }

    device(req, res){
        if(req.method == "POST"){
            r.table('devices').insert(
                {
                    "refresh_time":refresh_time,
                    coordinates: {
                        lat: 0.11,
                        long: 0.12
                    }
                }).run(connection, function(err, call){

                if(err){
                    console.log(err);
                }else{
                    let device_id = call.generated_keys;
                    if(device_id){
                        res.json({"device_id": device_id[0]});
                    }else{
                        res.status(500);
                    }
                }

            });

        }else if(req.method == "GET"){

            let id = req.params.id;

            if(id) {

                r.table('devices').get(id).run(connection, function(err, data){
                    console.log(data);
                });

                res.json({
                    device_id: id,
                    coordinates: {
                        lat: 0.11,
                        long: 0.12
                    },
                    refresh_time: 30000
                });

            }
        }
    }

    reading(req, res){
        if(req.method == "POST"){

            let id = req.params.device_id;

            let currentUnixTime = Date.now();
            let body = req.body;

            //TODO Schema check

            let data = {
                "device_id" : id ,
                "time_stamp" : currentUnixTime,
                body
            };
            console.log(data);
            r.table('readings').insert(data).run(connection, function(err, call){
                    if(err){
                        console.log(err);
                    }else{
                        res.json(data);
                    }
            });

        }else if(req.method == "GET"){
            res.json({
                data: [
                    {
                        "hardware_id" : "/dev/sda1",
                        "sensor_info" : [
                            {
                                "tag" : "cpu0_max",
                                "value": 12.32
                            },
                            {
                                "tag" : "cpu1_max",
                                "value": 10.32
                            },
                            {
                                "tag" : "cpu2_max",
                                "value": 1.32
                            },
                            {
                                "tag" : "cpu3_max",
                                "value": 14.32
                            }


                        ]
                    },
                    {
                        "hardware_id" : "/dev/sda2",
                        "sensor_info" : [
                            {
                                "tag" : "cpu0_max",
                                "value": 122.32
                            },
                            {
                                "tag" : "cpu1_max",
                                "value": 102.32
                            },
                            {
                                "tag" : "cpu2_max",
                                "value": 12.32
                            },
                            {
                                "tag" : "cpu3_max",
                                "value": 124.32
                            }
                        ]
                    }
                ]
            })

        }
    }

};


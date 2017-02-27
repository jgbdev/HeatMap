"use strict";
let r = require('rethinkdb');

let connection = null;

module.exports =  class DatabaseInterface {

    constructor(){
        this.device_id = 0;
        //this.connect(this);

    }


    connect(me){

        r.connect({
            host: 'localhost',
            port: '32769',
            db : 'test'
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
    }

    device(req, res){
        if(req.method == "POST"){

            res.json({
                "device_id" : this.device_id ++
            });
        }else if(req.method == "GET"){

            let id = req.params.id;

            if(id) {

                let id_val = parseInt(id);

                res.json({
                    device_id: id_val,
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
            res.json({});
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


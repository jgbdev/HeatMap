"use strict";


var r = require('rethinkdb');


module.exports =  class DatabaseInterface {

    constructor(){
        this.device_id = 0;
    }


    setupTables(){

        //r.db.('test'.tableCreate('table1',  {primaryKey: 'device_id'}).run()
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

        }
    }

};


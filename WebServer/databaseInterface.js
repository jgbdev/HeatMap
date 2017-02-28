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
            }else{
                r.table('readings').indexCreate('time_stamp').run(connection, function(err,conn){
                  if(err){
                      console.log(err);
                  }
                });
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
                console.log(id);
                r.table('devices').get(id).run(connection, function(err, data){

                    if(err){
                        console.log(err);
                        res.status(500);
                    }else{
                        console.log(data);
                        res.json(data);
                    }
                });

            }else{

                r.table('devices').pluck("device_id").run(connection, function(err, cursor) {
                    if (err) throw err;
                    cursor.toArray(function(err, result) {
                        if (err) throw err;
                        res.json(result);
                    });
                });

            }
        }else if(req.method == "PATCH"){

            let id = req.params.id;

            let body = req.body;
            console.log(body);


            r.table('devices').get(id).update(body).run(connection, function(err, data) {

                if(err){
                    res.json(500);
                }else{
                    res.json(body);
                }

            });


        }
    }

    reading_single(req, res){
        if(req.method == "POST"){

            let id = req.params.id;

            let currentUnixTime = Date.now();
            let data = req.body.data;

            //TODO Schema check

            let dd = {
                "device_id" : id ,
                "time_stamp" : currentUnixTime,
                data
            };
            console.log(dd);
            r.table('readings').insert(dd).run(connection, function(err, call){
                    if(err){
                        console.log(err);
                    }else{
                        res.json(data);
                    }
            });

        }else if(req.method == "GET"){
            let id = req.params.id;

            if(id) {
                r.table('readings').orderBy({index: r.desc('time_stamp')}).filter({"device_id":  id}).run(connection, function (err, cursor) {
                    if (err) throw err;
                    cursor.toArray(function(err, result) {
                        if (err) throw err;
                        console.log(result);
                        if(result.length == 0){
                            res.json({});
                        }else{
                            res.json(result[0]);
                        }
                    });
                });

            }

        }
    }


    reading_range(req, res){
        let id = req.params.id;
        let from = req.params.from;
        let to = req.params.to;
        if(id && from){

            let max = Date.now() + 1;

            //TODO int checking

            if(to){
                max = parseInt(to);
            }

            let from_int = parseInt(from);


            console.log(from);
                r.table('readings').between(from_int, max, { 'index' : 'time_stamp'}).run(connection, function (err, cursor){
                    if (err) throw err;
                    cursor.toArray(function(err, result) {
                        if (err) throw err;
                        console.log(result);
                        res.json(result);
                    });

                });

        }else{
            res.status(500);
        }
    }

};


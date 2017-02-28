"use strict";
let express = require("express");
let DatabaseInterface = require('./databaseInterface');
let app = express();

let bodyParser = require('body-parser')

let dbInterface = new DatabaseInterface();

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());


//DEVICE APIS
app.post("/api/device/", function(req, res) {
    console.log("POST: api/device");
    dbInterface.device(req, res);
});

app.get("/api/device/:id", function(req, res) {
    console.log("GET: api/device");
    dbInterface.device(req, res);
});

app.patch("/api/device/:id", function(req, res) {
    console.log("PATCH: api/device");
    dbInterface.device(req, res);
});

app.get("/api/device", function(req, res) {
    console.log("GET: api/device");
    dbInterface.device(req, res);
});

//READING APIS
app.get('/api/reading/:id', function (req, res){
    console.log("GET: Reading");
    dbInterface.reading_single(req,res);
});

app.post("/api/reading/:id", function(req, res) {
    console.log("POST: Reading");
    dbInterface.reading_single(req,res);
});

app.get("/api/reading/:id/from/:from", function(req, res) {
    console.log("GET: Reading /from ");
    dbInterface.reading_range(req,res);
});

app.get("/api/reading/:id/from/:from/to/:to", function(req, res) {
    console.log("GET: Reading /from/to");
    dbInterface.reading_range(req,res);
});



app.get('/', function (req, res) {
    res.status(404)
});


var port = process.env.PORT || 5000;
app.listen(port, function() {
    console.log("Listening on " + port);
});



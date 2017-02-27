let express = require("express");
var DatabaseInterface = require('./databaseInterface');
let app = express();

let bodyParser = require('body-parser')

let dbInterface = new DatabaseInterface();

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.post("/api/reading/:device_id", function(req, res) {
    console.log("POST: Reading");
    dbInterface.reading(req,res);
});


app.post("/api/reading/:device_id", function(req, res) {
    console.log("POST: Reading");
    dbInterface.reading(req,res);
});

app.post("/api/device/", function(req, res) {
    console.log("POST: api/device");
    dbInterface.device(req, res);
});




var port = process.env.PORT || 5000;
app.listen(port, function() {
    console.log("Listening on " + port);
});



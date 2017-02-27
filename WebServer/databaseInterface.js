
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

        }
    }

    reading(req, res){
        if(req.method == "POST"){


        }else if(req.method == "GET"){

        }
    }

};


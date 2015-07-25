function WebState(){

    this.Url;
    this.CurrentPage;
    this.Application;

    this.Init = function(){
        try{
            this.Url = document.getElementById("ctl00_HUrl").value;
            this.CurrentPage = document.getElementById("ctl00_HCurrentPage").value;
            this.Application = document.getElementById("ctl00_HApplication").value;
        }catch(Err){
            this.Url = null;
            this.CurrentPage = null;
            this.Application = null;
        }
    }
    
    //this.Init();
}
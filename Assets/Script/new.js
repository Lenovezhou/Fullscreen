
var round : Texture2D;  

var x = 0;
var y = 0;

var cross_x = 0;
var cross_y = 0;

function Start(){
    cross_x = Screen.width -  round.width;  
    cross_y = Screen.height -  round.height;  	
}

function OnGUI () {

	GUI.Label(Rect(0,0,480,100),"position is " + Input.acceleration);
	
	GUI.DrawTexture(Rect(x,y,256,256),round);   
}

function Update(){
	x += Input.acceleration.x * 30;
	y += -Input.acceleration.y * 30;	
	
	if(x < 0){  
        x = 0;  
    }else if(x > cross_x){  
        x = cross_x;  
    }  
      
    if(y < 0){  
        y = 0;  
    }else if(y > cross_y){  
        y = cross_y;  
    }  
}
#pragma strict

var myTimer : float = 10.0;
     
function Update () {
	if(myTimer > 0){
		myTimer -= Time.deltaTime;
	}
	
	if(myTimer <= 0){
		gameObject.particleSystem.Play();
		myTimer = 10;
	}
}

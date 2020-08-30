


DB details : 

tblUser : 
_id
name
mobileNumber
photoPath 

tblVehicle : 

_id  // assumed unique vehicle num for each vehicle 
vehicleType   // Type of vehicle Bike 
yearOfManu    // year of manufacturing, range query can be done.  
deviceId      // device num, here assumed same as vehicle 
userId        // manual ref to tblUser user100"}


App details : 

Genral : 

    Both API basics implemented and certain things which are not done like loggin exceptions and configs reading due to limitted
    time available. All the code is self written and hardly something referred from internet. 
    None is copied from anywhere. 
    Mongo DB DAL 

1. User controller : 
	
	add/edit basics implemented. Images can be stored/updated. 
	path of the image is assumed at server level with image actual name is user id name value with .format. 

2. Vehicle controller : 
	
	Single term equal search is implemented, however it can be updated to range search also. 




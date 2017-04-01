#target AfterEffects

/**************************************
Scene : Scene
Resolution : 1920 x 1080
Duration : 10.416667
FPS : 24.000000
Date : 2012-06-08 12:40:31.722000
Exported with io_export_after_effects.py
**************************************/



function compFromBlender(){

var compName = prompt("Blender Comp's Name \nEnter Name of newly created Composition","BlendComp","Composition's Name");
if (compName){
var newComp = app.project.items.addComp(compName, 1920, 1080, 1.000000, 10.416667, 24);
newComp.displayStartTime = 0.083333;


// **************  CAMERA 3D MARKERS  **************


// **************  OBJECTS  **************


var _Cylinder = newComp.layers.addNull();
_Cylinder.threeDLayer = true;
_Cylinder.source.name = "_Cylinder";
_Cylinder.property("position").setValue([960.000000,540.000000,0.000000],);
_Cylinder.property("orientation").setValue([-90.000009,-0.000008,-110.020339],);
_Cylinder.property("scale").setValue([224.287295,224.287295,224.287295],);


// **************  LIGHTS  **************


// **************  CAMERAS  **************


var _Camera = newComp.layers.addCamera("_Camera",[0,0]);
_Camera.autoOrient = AutoOrientType.NO_AUTO_ORIENT;
_Camera.property("position").setValue([1745.080147,133.089962,-1092.567444],);
_Camera.property("orientation").setValue([-20.313171,-35.067574,-12.004430],);
_Camera.property("zoom").setValue(2100.000000,);



}else{alert ("Exit Import Blender animation data \nNo Comp's name has been chosen","EXIT")};}


app.beginUndoGroup("Import Blender animation data");
compFromBlender();
app.endUndoGroup();



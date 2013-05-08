#pragma once

#include "ofMain.h"

#include "ofxEdsdk.h"
#include "ofxOsc.h"

class testApp : public ofBaseApp {
public:
	void setup();
	void update();
	void draw();
	
	ofxEdsdk::Camera camera;
	ofxOscReceiver oscIn;
	ofxOscSender oscOut;
	
	string savePath;
	bool capturing;
};

#pragma once

#include "ofMain.h"
#include "ofxAssimpModelLoader.h"
#include "ofxCv.h"
#include "ofAutoShader.h"
#include "ofxBlackMagic.h"
#include "CoordWarp.h"
#include "ofxXmlSettings.h"

enum Stage {
    Lighthouse=0,
    Spotlight,
    Intermezzo
} ;

class ofApp : public ofBaseApp {
public:
	void setup();
	void update();
	void draw();
	void keyPressed(int key);
    void mouseMoved(int x, int y);
    void mousePressed( int x, int y, int button );
    
    
    void updateCameraCalibration();
    
    bool debugMode;
	
	ofFloatImage xyzMap;
    ofFloatImage normalMap;
    ofFloatImage confidenceMap;
	ofAutoShader shader;
    
    //Settings
    ofxXmlSettings settings;
    
    Stage stage;
    Stage stageGoal;
    
    float stageAge;
    float stageAmp;

    //Lighthouse
    float lighthouseAngle;

    //Intermezzo
    float intermezzoTimer;
    
    
    
    //Speaker sampling
    ofFloatImage speakerXYZMap;
    ofFbo speakerFbo;
    ofFloatPixels speakerPixels;

    
    float speakerAmp[4];
    
    //Tracking
    ofxBlackMagic grabber;
    
    ofxCv::RunningBackground cameraBackground;
    ofxCv::ContourFinder contourFinder;
    ofImage thresholdedImage;
    coordWarping cameraCalibration;
    ofVec2f cameraCalibrationCorners[4];
    bool firstFrame;
    int setCorner;
};
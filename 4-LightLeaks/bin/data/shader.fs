#version 120

#define PI (3.1415926536)
#define TWO_PI (6.2831853072)
#define HALF_PI (PI*0.5)

uniform sampler2DRect xyzMap;
uniform sampler2DRect normalMap;
uniform sampler2DRect confidenceMap;
uniform int useConfidence;

uniform float elapsedTime;

//Lighthouse
uniform float beamAngle;
uniform float beamWidth;

//Spotlight
uniform float spotlightSize = 0.2;
uniform vec2 spotlightPos = vec2(0.5,0.5);

uniform int stage = 0;

const vec3 center = vec3(0.1, 0.25, 0.5);

void main() {
    vec2 projectionOffset = vec2(0);//+vec2(floor( sin(elapsedTime*1)*10) ,0);
    vec3 position = texture2DRect(xyzMap, gl_TexCoord[0].st + projectionOffset).xyz;
    vec3 centered = position - center;
    vec3 normal = texture2DRect(normalMap, gl_TexCoord[0].st + projectionOffset).xyz;
    float confidence = texture2DRect(confidenceMap, gl_TexCoord[0].st).r;
    if(useConfidence == 0) {
        confidence = 1.;
    }
    
    if(confidence < .1) {
        gl_FragColor = vec4(vec3(0), 1);
        return;
    }
    
    float b = 0.;
    
    // handle time
    float time = elapsedTime;
    time = elapsedTime + sin(elapsedTime); // step time
    
    // handle space
    vec2 positionNorm = position.xy;
    float positionAngle = atan((positionNorm.y - 0.25) , (positionNorm.x-0.5)) + PI + HALF_PI;
    
//    if(stage == 0) {
//        //Lighthouse beam
//        
//        if(abs(positionAngle - beamAngle) < beamWidth
//           || abs(positionAngle - TWO_PI - beamAngle) < beamWidth
//           || abs(positionAngle + TWO_PI - beamAngle) < beamWidth){
//            b = 1.;
//        } else {
//            b = 0.;
//        }
//        
//      //  if(confidence < .01) discard;
//    }
//    else if(stage == 1){
//        //Spotlight
//        if( position.z > 0.15 &&
//           length(positionNorm.xy - spotlightPos) < spotlightSize){
//            b = 1.;
//        } else {
//            b = 0.;
//        }
//    }
//    else if(stage == 2){
    
    float substage = mod(elapsedTime / 10., 4.);
    if(substage < 1) {
        // fast rising stripes
        b = mod(position.x * 10. - time * 1.5, 1.);
        b *= b;
    } else if(substage < 2) {
        // glittering floor
        float t = sin(time)*.5;
        vec2 rot = vec2(sin(t), cos(t)) * (1. + sin(time) * .5) + time;
        b = sin(50. * dot(rot, centered.yz));
    } else if(substage < 3) {
        // concentric spheres
        b = sin(500.*mod(length(centered)+(0.02*sin(time*1.)), 10.));
    } else if(substage < 4) {
        // unstable floor
        float t = sin(time)*.1;
        vec2 rot = vec2(sin(t), cos(t));
        b = sin(50.*dot(rot, centered.yz));
    } else if(substage < 5) {
        // checkerboard (needs to be animated)
        vec3 modp = mod(position.xyz * 24, 2.);
        if(modp.x > 1) {
            b = (modp.z < 1 || modp.y > 1) ? 1 : 0;
        } else {
            b = (modp.z > 1 || modp.y < 1) ? 1 : 0;
        }
    }
    
//    }
    
    
    // threshold
//    if(bw) {
//        b = b > .5 ? 1 : 0;
//    }
    
//    b *= confidence; // for previs
    gl_FragColor = vec4(vec3(b), 1.);
}


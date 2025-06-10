#version 330 core
out vec4 FragColor;

in vec3 WorldPos;

uniform samplerCube environmentMap;

const float PI = 3.14159265359;
void main()
{
    vec3 normal = normalize(WorldPos);

    vec3 irradiance = vec3(0.0);

    vec3 up = vec3(0.0, 1.0, 0.0);
    vec3 right = normalize(cross(up, normal));
    up = normalize(cross(normal, right));

    float sampleDelta = 0.025;
    float nrSamples = 0.0;

    for(float phi = 0.0; phi < 2*PI; phi+=sampleDelta)
    {
        for(float theta = 0.0; theta <0.5*PI; theta += sampleDelta)
        {
            vec3 tangentSample = vec3(cos(theta) * sin(phi), sin(theta), cos(theta) * cos(phi));
            vec3 sampleVec = tangentSample.x * right + tangentSample.y * up + tangentSample.z * normal;

            irradiance += texture(environmentMap, sampleVec).rgb * cos(theta) * sin(theta);
            nrSamples += 1.0;
        }
    }
    irradiance = PI * irradiance / nrSamples;
    FragColor = vec4(irradiance, 1.0);
}
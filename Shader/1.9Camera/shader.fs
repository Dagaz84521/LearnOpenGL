#version 330 core
out vec4 FragColor;
in vec3 ourColor;
uniform float r;
uniform float g;
uniform float b;
void main()
{
   FragColor = vec4(ourColor.x - r,ourColor.y - g,ourColor.z - b,1.0f);
}
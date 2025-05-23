#version 330 core
layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;

in VS_OUT{
    vec2 texCoords;
    vec3 position;
}gs_in[];

out vec2 TexCoords;

uniform float time;
uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

vec3 GetNormal()
{
    vec3 a = vec3(gl_in[0].gl_Position) - vec3(gl_in[1].gl_Position);
    vec3 b = vec3(gl_in[2].gl_Position) - vec3(gl_in[1].gl_Position);
    vec3 normal = normalize(cross(b, a));
    return normal;
}


vec4 explode(vec4 position, vec3 normal)
{
    float magnitude = 2.0;
    vec3 direction = normal * ((sin(time) + 1.0) / 2.0) * magnitude; 
    return position + vec4(direction, 0.0);
}

void main()
{
    vec3 normal = GetNormal();
    mat4 mvp = projection * view * model;
    gl_Position = mvp * explode(gl_in[0].gl_Position, normal);
    TexCoords = gs_in[0].texCoords;
    EmitVertex();
    gl_Position = mvp * explode(gl_in[1].gl_Position, normal);
    TexCoords = gs_in[1].texCoords;
    EmitVertex();
    gl_Position = mvp * explode(gl_in[2].gl_Position, normal);
    TexCoords = gs_in[2].texCoords;
    EmitVertex();
    EndPrimitive();
}
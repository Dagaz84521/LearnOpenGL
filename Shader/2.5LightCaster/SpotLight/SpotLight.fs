#version 330 core
struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
}; 

struct Light{
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};


out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

uniform vec3 viewPos;
uniform Material material;
uniform Light light;

void main()
{
    vec3 lightDir = normalize(light.position - FragPos); // Directional light direction is inverted
    
    vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;

    vec3 norm = normalize(Normal);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;
    
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

    float theta = dot(lightDir, normalize(-light.direction)); // Angle between light direction and normal
    float epsilon = light.cutOff - light.outerCutOff; // Angle between inner and outer cut-off
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    
    diffuse  *= intensity;
    specular *= intensity;
    // attenuation
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    

    // ambient  *= attenuation; // remove attenuation from ambient, as otherwise at large distances the light would be darker inside than outside the spotlight due the ambient term in the else branch
    diffuse   *= attenuation;
    specular *= attenuation;   


    vec3 result = ambient + diffuse + specular;

    FragColor = vec4(result, 1.0);

}
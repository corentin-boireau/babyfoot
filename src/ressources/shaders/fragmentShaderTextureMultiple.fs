#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

uniform Material material;

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float strength;
};

uniform Light light; 
uniform bool lightSource;

uniform vec3 cameraPos;

out vec4 FragColor;

void main()
{
    if(lightSource) {
        FragColor = light.strength * vec4(material.ambient + material.diffuse + material.specular, 1.0f);
        return;
    }

    // ambient lighting calculations
    vec3 ambient = light.ambient;

    // diffuse lighting calculations
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * (light.diffuse *  material.diffuse);

    // specular lighting calculations
    vec3 viewDir = normalize(cameraPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec  * material.specular);  

    FragColor = light.strength * vec4(ambient + diffuse + specular, 1.0f);
}
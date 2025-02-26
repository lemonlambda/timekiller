shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;

uniform vec2 resolution = vec2(1920.0, 1080.0);
uniform float pixel_density :hint_range(1.0, 4.0) = 2.0; // 💥 Increase pixel density

uniform float scan_line_amount :hint_range(0.0, 3.0) = 0.4;
uniform float warp_amount :hint_range(0.0, 5.0) = 0.05;
uniform float noise_amount :hint_range(0.0, 0.3) = 0.0;
uniform float interference_amount :hint_range(0.0, 1.0) = 0.0;
uniform float grille_amount :hint_range(0.0, 1.0) = 0.05;
uniform float grille_size :hint_range(1.0, 5.0) = 2.0;
uniform float vignette_amount :hint_range(0.0, 2.0) = 0.5;
uniform float vignette_intensity :hint_range(0.0, 1.0) = 0.3;
uniform float aberation_amount :hint_range(0.0, 1.0) = 0.2;
uniform float roll_line_amount :hint_range(0.0, 1.0) = 0.0;
uniform float roll_speed :hint_range(-8.0, 8.0) = 1.0;
uniform float scan_line_strength :hint_range(-12.0, -1.0) = -5.0;
uniform float pixel_strength :hint_range(-4.0, 0.0) = -1.5;

float random(vec2 uv) {
    return fract(cos(uv.x * 83.4827 + uv.y * 92.2842) * 43758.5453123);
}

vec3 fetch_pixel(vec2 uv, vec2 off) {
    // Apply pixel density scaling
    vec2 pos = floor(uv * resolution * pixel_density + off) / (resolution * pixel_density) + vec2(0.5) / (resolution * pixel_density);
    float noise = noise_amount > 0.0 ? random(pos + fract(TIME)) * noise_amount : 0.0;
    if (max(abs(pos.x - 0.5), abs(pos.y - 0.5)) > 0.5) return vec3(0.0);
    return texture(SCREEN_TEXTURE, pos, -16.0).rgb + noise;
}

vec2 Dist(vec2 pos) {
    pos *= resolution * pixel_density;
    return -((pos - floor(pos)) - vec2(0.5));
}

float Gaus(float pos, float scale) {
    return exp2(scale * pos * pos);
}

vec3 Horz3(vec2 pos, float off) {
    vec3 b = fetch_pixel(pos, vec2(-1.0, off));
    vec3 c = fetch_pixel(pos, vec2(0.0, off));
    vec3 d = fetch_pixel(pos, vec2(1.0, off));
    float dst = Dist(pos).x;
    float scale = pixel_strength;
    float wb = Gaus(dst - 1.0, scale);
    float wc = Gaus(dst + 0.0, scale);
    float wd = Gaus(dst + 1.0, scale);
    return (b * wb + c * wc + d * wd) / (wb + wc + wd);
}

float Scan(vec2 pos, float off) {
    return Gaus(Dist(pos).y + off, scan_line_strength);
}

vec3 Tri(vec2 pos) {
    vec3 clr = fetch_pixel(pos, vec2(0.0));
    if (scan_line_amount > 0.0) {
        vec3 a = Horz3(pos, -1.0);
        vec3 b = Horz3(pos, 0.0);
        vec3 c = Horz3(pos, 1.0);
        float wa = Scan(pos, -1.0);
        float wb = Scan(pos, 0.0);
        float wc = Scan(pos, 1.0);
        clr = mix(clr, a * wa + b * wb + c * wc, scan_line_amount);
    }
    return clr;
}

vec2 warp(vec2 uv) {
    vec2 delta = uv - 0.5;
    float delta2 = dot(delta, delta);
    float delta4 = delta2 * delta2;
    return (uv + delta * delta4 * warp_amount - 0.5) / mix(1.0, 1.2, warp_amount / 5.0) + 0.5;
}

float vignette(vec2 uv) {
    uv *= 1.0 - uv.xy;
    float v = uv.x * uv.y * 15.0;
    return pow(v, vignette_intensity * vignette_amount);
}

vec3 grille(vec2 uv) {
    float unit = PI / 3.0;
    float scale = 2.0 * unit / grille_size;
    float r = smoothstep(0.5, 0.8, cos(uv.x * scale - unit));
    float g = smoothstep(0.5, 0.8, cos(uv.x * scale + unit));
    float b = smoothstep(0.5, 0.8, cos(uv.x * scale + 3.0 * unit));
    return mix(vec3(1.0), vec3(r, g, b), grille_amount);
}

void fragment() {
    vec2 pos = warp(SCREEN_UV);
    vec3 clr = Tri(pos);

    if (aberation_amount > 0.0) {
        float chroma = aberation_amount;
        vec2 chromatic_x = vec2(chroma, 0.0) / resolution.x;
        vec2 chromatic_y = vec2(0.0, chroma / 2.0) / resolution.y;
        float r = Tri(pos - chromatic_x).r;
        float g = Tri(pos + chromatic_y).g;
        float b = Tri(pos + chromatic_x).b;
        clr = vec3(r, g, b);
    }

    if (grille_amount > 0.0) clr *= grille(FRAGCOORD.xy);
    clr *= 1.0 + scan_line_amount * 0.6;
    if (vignette_amount > 0.0) clr *= vignette(pos);

    COLOR.rgb = clr;
    COLOR.a = 1.0;
}

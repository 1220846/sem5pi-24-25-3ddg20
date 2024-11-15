import * as THREE from "three";

/*
 * parameters = {
 *  ambientLight: { color: Integer, intensity: Float },
 *  pointLight1: { color: Integer, intensity: Float, distance: Float, position: Vector3 },
 *  pointLight2: { color: Integer, intensity: Float, distance: Float, position: Vector3 },
 *  spotLight: { color: Integer, intensity: Float, distance: Float, angle: Float, penumbra: Float, position: Vector3, direction: Float }
 * }
 */

export default class Lights {
    constructor(parameters) {
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }

        // Create a group of objects
        this.object = new THREE.Group();

        // Create the ambient light
        this.object.ambientLight = new THREE.AmbientLight(this.ambientLight.color, this.ambientLight.intensity);

        this.object.add(this.object.ambientLight);

        this.object.directionalLight1 = new THREE.DirectionalLight(this.directionalLight1.color, this.directionalLight1.intensity);
        this.object.directionalLight1.position.set(this.directionalLight1.position.x, this.directionalLight1.position.y, this.directionalLight1.position.z);
        this.object.directionalLight1.castShadow = true;

        this.object.directionalLight1.shadow.mapSize.width = 512;
        this.object.directionalLight1.shadow.mapSize.height = 512;
        this.object.directionalLight1.shadow.camera.near = 5.0;
        this.object.directionalLight1.shadow.camera.far = 15.0;

        this.object.add(this.object.directionalLight1);

        this.object.directionalLight2 = new THREE.DirectionalLight(this.directionalLight2.color, this.directionalLight2.intensity);
        this.object.directionalLight2.position.set(this.directionalLight2.position.x, this.directionalLight2.position.y, this.directionalLight2.position.z);
        this.object.directionalLight2.castShadow = true;

        this.object.directionalLight2.shadow.mapSize.width = 512;
        this.object.directionalLight2.shadow.mapSize.height = 512;
        this.object.directionalLight2.shadow.camera.near = 5.0;
        this.object.directionalLight2.shadow.camera.far = 15.0;

        this.object.add(this.object.directionalLight2);
    }
}
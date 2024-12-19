import * as THREE from "three";
import * as TWEEN from "@tweenjs/tween.js";
import Ground from "./ground.js";
import Wall from "./wall.js";
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { parameter, texture } from "three/webgpu";

/*
 * parameters = {
 *  url: String,
 *  credits: String,
 *  scale: Vector3
 * }
 */

export default class Maze {
    constructor(parameters, camera, renderer, scene3D) {

        this.onLoad = function (description) {

            // Create RayCaster
            this.raycaster = new THREE.Raycaster();
            this.mouse = new THREE.Vector2();
            this.camera = camera;
            this.defaultcamera=camera;
            this.renderer = renderer;
            this.scene3D = scene3D;

            // Store the maze's map and size
            this.map = description.map;
            this.size = description.size;


            // Store the player's initial position and direction
            //this.initialPosition = this.cellToCartesian(description.initialPosition);
            //this.initialDirection = description.initialDirection;


            // Store the maze's exit location
            this.exitLocation = this.cellToCartesian(description.exitLocation);


            // Create a group of objects
            this.object = new THREE.Group();

            // Create the ground
            this.ground = new Ground({ textureUrl: description.groundTextureUrl, size: description.size });
            this.object.add(this.ground.object);

            // Create a wall
            this.wall = new Wall({ textureUrl: description.wallTextureUrl });


            // Read whih rooms are busy
            const busyRooms = new Set(description.busyRooms);


            // Build the maze
            let wallObject;
            for (let i = 0; i <= description.size.width; i++) { // In order to represent the eastmost walls, the map width is one column greater than the actual maze width
                for (let j = 0; j <= description.size.height; j++) { // In order to represent the southmost walls, the map height is one row greater than the actual maze height
                    /*
                     * description.map[][] | North wall | West wall
                     * --------------------+------------+-----------
                     *          0          |     No     |     No
                     *          1          |     No     |    Yes
                     *          2          |    Yes     |     No
                     *          3          |    Yes     |    Yes
                     *         400         |    Yes     |  Main Door
                     *         4xx         |     No     |  Door #xx
                     *         5xx         |    Operating Table #xx
                     */
                    // To-do #5 - Create the north walls
                    if (description.map[j][i] == 2 || description.map[j][i] == 3 || description.map[j][i] == 400) {
                        wallObject = this.wall.object.clone();
                        wallObject.position.set(i - description.size.width / 2.0 + 0.5, 0.5, j - description.size.height / 2.0);
                        this.object.add(wallObject);
                    }
                    // To-do #6 - Create the west walls
                    if (description.map[j][i] == 1 || description.map[j][i] == 3) {
                        wallObject = this.wall.object.clone();
                        wallObject.rotateY(Math.PI / 2.0);
                        wallObject.position.set(i - description.size.width / 2.0, 0.5, j - description.size.height / 2.0 + 0.5);
                        this.object.add(wallObject);
                    }
                    // Doors
                    if (Math.floor(description.map[j][i] / 100) == 4) {
                        const loader = new GLTFLoader();
                        const loadDoorPromise = new Promise((resolve, reject) => {
                            loader.load("/models/gltf/Doors/double_doors.glb", (glb) => {
                                this.door = { object: glb.scene };
                                console.log("Door loaded successfully:", this.door);
                                resolve();
                            }, undefined, (error) => {
                                console.error(`Error loading door model (${error}).`);
                                reject(error);
                            });
                        });
                        const doorId = description.map[j][i] % 100;
                        loadDoorPromise.then(() => {
                            this.object.add(this.door.object);
                            this.door.object.scale.set(0.0032, 0.0029, 0.0032);
                            this.door.object.position.set(i - description.size.width / 2.0, 0.0, j - description.size.height / 2.0 + 0.5);
                            this.door.object.rotation.y = (Math.PI * (doorId % 2 == 0 ? 1 : 3)) / 2;
                        }).catch((error) => {
                            console.error("Error loading door model:", error);
                        });
                    }
                    // Tables
                    if (Math.floor(description.map[j][i] / 100) == 5) {
                        const loader = new GLTFLoader();
                        const loadTablePromise = new Promise((resolve, reject) => {
                            loader.load("/models/gltf/Table/hospital_bed2.glb", (glb) => {
                                this.table = { object: glb.scene };
                                console.log("Table loaded successfully:", this.table);
                                resolve();
                            }, undefined, (error) => {
                                console.error(`Error loading table model (${error}).`);
                                reject(error);
                            });
                        });
                        const tableId = description.map[j][i] % 100;
                        loadTablePromise.then(() => {
                            this.object.add(this.table.object);
                            this.table.object.scale.set(0.7, 0.7, 0.7);
                            this.table.object.position.set(i - description.size.width / 2.0 - 1.9, 0.0, j - description.size.height / 2.0 - 3.125);

                            // Box for click
                            const boxTable = this.createBoxTable(this.table.object, 2.5);
                            boxTable.name = `Table_${tableId}`;
                            this.object.add(boxTable);

                       }).catch((error) => {
                            console.error("Error loading table model:", error);
                        });

                        // Patients
                        if (busyRooms.has(tableId)) {
                            const loader = new GLTFLoader();
                            const loadPatientPromise = new Promise((resolve, reject) => {
                                loader.load("/models/gltf/Patient/patient.glb", (glb) => {
                                    this.patient = { object: glb.scene };
                                    console.log("Patient loaded successfully:", this.patient);
                                    resolve();
                                }, undefined, (error) => {
                                    console.error(`Error loading patient model (${error}).`);
                                    reject(error);
                                });
                            });
                            loadPatientPromise.then(() => {
                                this.object.add(this.patient.object);
                                this.patient.object.scale.set(0.007, 0.01, 0.007);
                                this.patient.object.position.set(i - description.size.width / 2.0 + 0.5 + 0.1 * (tableId % 2 == 0 ? 1 : -1), 0.6, j - description.size.height / 2.0 + 0.5);
                                this.patient.object.rotation.y = (Math.PI * (tableId % 2 == 0 ? 3 : 1)) / 2;
                            });


                            const loader2 = new GLTFLoader();
                            const loadDoctorPromise = new Promise((resolve, reject) => {
                                loader2.load("/models/gltf/Doctor/doctor_pinch.glb", (glb) => {
                                    this.doctor = { object: glb.scene };
                                    console.log("Doctor loaded successfully:", this.doctor);
                                    resolve();
                                }, undefined, (error) => {
                                    console.error(`Error loading doctor model (${error}).`);
                                    reject(error);
                                })
                            });
                            loadDoctorPromise.then(() => {
                                this.object.add(this.doctor.object);
                                this.doctor.object.scale.set(0.005, 0.009, 0.005);
                                this.doctor.object.position.set(i - description.size.width / 2.0 + 0.5 + 0.1 * (tableId % 2 == 0 ? 1 : -1), 0.0, j - description.size.height / 2.0 + 0.5 + 0.6);
                                this.doctor.object.rotation.y = (Math.PI * (tableId % 2 == 0 ? 3 : 1));
                            });
                        }
                    }
                }
            }

            this.object.scale.set(this.scale.x, this.scale.y, this.scale.z);
            this.loaded = true;
        }

        this.onProgress = function (url, xhr) {
            console.log("Resource '" + url + "' " + (100.0 * xhr.loaded / xhr.total).toFixed(0) + "% loaded.");
        }

        this.onError = function (url, error) {
            console.error("Error loading resource " + url + " (" + error + ").");
        }

        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        this.loaded = false;

        // The cache must be enabled; additional information available at https://threejs.org/docs/api/en/loaders/FileLoader.html
        THREE.Cache.enabled = true;

        // Create a resource file loader
        const loader = new THREE.FileLoader();

        // Set the response type: the resource file will be parsed with JSON.parse()
        loader.setResponseType("json");

        // Load a maze description resource file
        loader.load(
            //Resource URL
            this.url,

            // onLoad callback
            description => this.onLoad(description),
            

            // onProgress callback
            xhr => this.onProgress(this.url, xhr),

            // onError callback
            error => this.onError(this.url, error)
        );
        window.addEventListener('click', this.onMouseClick);

        this.selectedRoom=null;
        this.infoOverlayVisible=false;
        this.initEventListeners();
    }

    // Convert cell [row, column] coordinates to cartesian (x, y, z) coordinates
    cellToCartesian(position) {
        return new THREE.Vector3((position[1] - this.size.width / 2.0 + 0.5) * this.scale.x, 0.0, (position[0] - this.size.height / 2.0 + 0.5) * this.scale.z)
    }

    // Convert cartesian (x, y, z) coordinates to cell [row, column] coordinates
    cartesianToCell(position) {
        return [Math.floor(position.z / this.scale.z + this.size.height / 2.0), Math.floor(position.x / this.scale.x + this.size.width / 2.0)];
    }

    createBoxTable(table, increaseFactor = 1) {
        const box = new THREE.Box3().setFromObject(table);
    
        // Get the size and center of the current box
        const size = new THREE.Vector3();
        const center = new THREE.Vector3();
        box.getSize(size);
        box.getCenter(center);
    
        // New Height
        const adjustedHeight = size.y * increaseFactor;
    
        // Create new geometry
        const geometry = new THREE.BoxGeometry(size.x, adjustedHeight, size.z);
        const material = new THREE.LineBasicMaterial({
            color: 0x808080,
            transparent: true,
            opacity: 0,
        });
        const boxMesh = new THREE.Mesh(geometry, material);
    
        // Adjust the position of the box to remain aligned
        boxMesh.position.copy(center);
        boxMesh.position.y += (adjustedHeight - size.y) / 2;
    
        return boxMesh;
    }    

    onMouseClick = (event) => {
    
        const rect = this.renderer.domElement.getBoundingClientRect();
        this.mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
        this.mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;
    
        this.raycaster.setFromCamera(this.mouse, this.camera.object);
        const intersects = this.raycaster.intersectObjects(this.scene3D.children, true);
    
        if (intersects.length > 0) {
            const clickedObject = intersects[0].object;
            console.log("Object clicked:", clickedObject.name);
    
            if (clickedObject.name && clickedObject.name.includes("Table")) {
                const tablePosition = clickedObject.position;
                console.log("Selected operating table:", clickedObject.name, "Position:", tablePosition);
    
                this.moveCameraToRoom(tablePosition, this.camera);
                this.selectedRoom=clickedObject.name.split('_')[1];
                
            } else {
                console.log("The clicked object is not a surgical table.");
                this.selectedRoom=null;
                this.hideRoomInfoOverlay();
            }
        } else {
            console.log("No objects were clicked.");
            this.selectedRoom=null;
            this.hideRoomInfoOverlay();
        }
    };

    initEventListeners() {
        window.addEventListener('keydown', (event) => {
            if (event.key.toLocaleLowerCase() === "i" && this.selectedRoom) {
                this.toggleRoomInfoOverlay();
            }
        });
    }

    toggleRoomInfoOverlay() {
        if (this.infoOverlayVisible) {
            this.selectedRoom=null;
            this.hideRoomInfoOverlay();
        } else {
            this.showRoomInfoOverlay();
        }
    }

    showRoomInfoOverlay() {
        if (!this.selectedRoom) return;
    
        let overlay = document.getElementById("roomInfoOverlay");
        if (!overlay) {
            overlay = document.createElement("div");
            overlay.id = "roomInfoOverlay";
            overlay.style.position = "fixed";
            overlay.style.top = "10px";
            overlay.style.left = "10px";
            overlay.style.backgroundColor = "rgb(255, 255, 255)";
            overlay.style.color = "black";
            overlay.style.padding = "10px";
            overlay.style.borderRadius = "5px";
            overlay.style.zIndex = "1000";
            overlay.style.width = "300px"; // Define a largura
            overlay.style.height = "400px"; // Define a altura
            document.body.appendChild(overlay);
        }
    
        overlay.innerHTML = `
            <h3>Room Information</h3>
            <p><strong>Name:</strong> </p>
            <p><strong>Position:</strong></p>
            <p><strong>Info:</strong></p>
        `;
        overlay.style.display = "block";
        this.infoOverlayVisible = true;
    }

    hideRoomInfoOverlay() {
        const overlay = document.getElementById("roomInfoOverlay");
        if (overlay) {
            overlay.style.display = "none";
        }
        this.infoOverlayVisible = false;
    }

    moveCameraToRoom(position, camera) {
        const [row, column] = this.cartesianToCell(position);
    
        // Calculate central coordinates relative to map size and scale
        const centerX = (column - this.size.width / 2.0 + 0.5) * this.scale.x;
        const centerZ = (row - this.size.height / 2.0 + 0.5) * this.scale.z;
    
        // Define fixed height position
        const targetPosition = new THREE.Vector3(centerX, 3.5, centerZ);
    
        // Create camera position tween
        new TWEEN.Tween(camera.object.position)
            .to({ x: targetPosition.x, y: targetPosition.y, z: targetPosition.z }, 1000)
            .easing(TWEEN.Easing.Quadratic.Out)
            .start();
    
        // Create look-at target
        const lookAtTarget = new THREE.Vector3(centerX, 0, centerZ);
        const initialRotation = camera.object.rotation.clone();
    
        // Create camera rotation tween
        new TWEEN.Tween(camera.object.rotation)
            .to({
                x: Math.atan2(lookAtTarget.z - camera.object.position.z, lookAtTarget.x - camera.object.position.x),
                y: initialRotation.y,
                z: 0
            }, 1000)
            .easing(TWEEN.Easing.Quadratic.Out)
            .onUpdate(() => {
                camera.object.lookAt(lookAtTarget);
            })
            .start();
    
        // Adjust camera up vector
        const targetUp = new THREE.Vector3(0, 0, -1);
        new TWEEN.Tween(camera.object.up)
            .to({ x: targetUp.x, y: targetUp.y, z: targetUp.z }, 1000)
            .easing(TWEEN.Easing.Quadratic.Out)
            .start();
    }

    /* To-do #23 - Measure the player’s distance to the walls
        - player position: position
    distanceToWestWall(position) {
        const indices = this.cartesianToCell(position);
        if (this.map[indices[0]][indices[1]] == 1 || this.map[indices[0]][indices[1]] == 3) {
            return position.x - this.cellToCartesian(indices).x + this.scale.x / 2.0;
        }
        return Infinity;
    }

    distanceToEastWall(position) {
        const indices = ...;
        indices[1]++;
        if (... || ...) {
            return ...;
        }
        return ...;
    }

    distanceToNorthWall(position) {
        const indices = ...;
        if (... || ...) {
            return ...;
        }
        return ...;
    }

    distanceToSouthWall(position) {
        const indices = ...;
        ...++;
        if (... || ...3) {
            return ...z;
        }
        return ...;
    } */

    foundExit(position) {
        return false;
        /* To-do #42 - Check if the player found the exit
            - assume that the exit is found if the distance between the player position and the exit location is less than (0.5 * maze scale) in both the X- and Z-dimensions
            - player position: position
            - exit location: this.exitLocation
            - maze scale: this.scale
            - remove the previous instruction and replace it with the following one (after completing it)
        return ... < ... && ... */
    };
}
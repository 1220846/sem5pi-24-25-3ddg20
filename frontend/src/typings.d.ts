declare module 'orientation';
declare module 'hospital' {
    export default class Hospital {
        constructor(params: any);
        update(): void;
        buildHelpPanel(): any;
        displayPanel(): any;
        setActiveViewCamera(): any;
        arrangeViewports(multipleViews): any;
        pointerIsOverViewport(pointer, viewport): any;
        getPointedViewport(pointer): any;
        setViewMode(multipleViews)
    }
}
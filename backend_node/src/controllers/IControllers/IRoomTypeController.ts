import { Request, Response, NextFunction } from 'express';

export default interface IRoomTypeController  {
    addRoomType(req: Request, res: Response, next: NextFunction);
    getRoomType(req: Request, res: Response, next: NextFunction);
    getRoomTypes(req: Request, res: Response, next: NextFunction);
}
import { Request, Response } from "express";
import RoomTypeController from "../../../src/controllers/roomTypeController"; 
import IRoomTypeService from "../../../src/services/IServices/IRoomTypeService";
import { describe, it, beforeEach, jest, expect } from '@jest/globals';
import { Result } from "../../../src/core/logic/Result";

jest.mock("../../../src/services/roomTypeService");

describe("RoomTypeController", () => {
  let roomTypeController: RoomTypeController;
  let roomTypeServiceMock: jest.Mocked<IRoomTypeService>;

  let req: Partial<Request>;
  let res: Partial<Response>;
  let next: jest.Mock;

  beforeEach(() => {
    roomTypeServiceMock = {
      addRoomType: jest.fn(),
      getRoomType: jest.fn(),
      getRoomTypes: jest.fn(),
    };

    roomTypeController = new RoomTypeController(roomTypeServiceMock);

    res = {
        status: jest.fn().mockReturnThis(),
        json: jest.fn().mockReturnThis(), 
    } as Partial<Response>;

    req = { body: { name: "Operating Room", id: "1" }, params: { id: "1" } };
    next = jest.fn();
  });

  describe("addRoomType", () => {
    it("should add a room type successfully", async () => {
      const roomTypeDTO = { name: "Operating Room", id: "1" };
      roomTypeServiceMock.addRoomType.mockResolvedValue(Result.ok(roomTypeDTO));

      await roomTypeController.addRoomType(req as Request, res as Response, next);

      expect(res.status).toHaveBeenCalledWith(201);
      expect(res.json).toHaveBeenCalledWith(roomTypeDTO);
      expect(roomTypeServiceMock.addRoomType).toHaveBeenCalledWith(req.body);
    });

    it("should return 402 on failure", async () => {
      roomTypeServiceMock.addRoomType.mockResolvedValue(Result.fail("Failed to add room type"));

      await roomTypeController.addRoomType(req as Request, res as Response, next);

      expect(res.status).toHaveBeenCalledWith(402);
    });

    it("should call next on error", async () => {
      const error = new Error("Unexpected error");
      roomTypeServiceMock.addRoomType.mockRejectedValue(error);

      await roomTypeController.addRoomType(req as Request, res as Response, next);

      expect(next).toHaveBeenCalledWith(error);
    });
  });

  describe("getRoomType", () => {
    it("should return a room type by ID", async () => {
      const roomTypeDTO = { name: "Operating Room", id: "1" };
      roomTypeServiceMock.getRoomType.mockResolvedValue(Result.ok(roomTypeDTO));

      await roomTypeController.getRoomType(req as Request, res as Response, next);

      expect(res.status).toHaveBeenCalledWith(200);
      expect(res.json).toHaveBeenCalledWith(roomTypeDTO);
    });

    it("should return 404 if room type is not found", async () => {
      roomTypeServiceMock.getRoomType.mockResolvedValue(Result.fail("Room Type not found"));

      await roomTypeController.getRoomType(req as Request, res as Response, next);

      expect(res.status).toHaveBeenCalledWith(404);
      expect(res.json).toHaveBeenCalledWith({ message: "Room Type not found" });
    });

    it("should call next on error", async () => {
      const error = new Error("Unexpected error");
      roomTypeServiceMock.getRoomType.mockRejectedValue(error);

      await roomTypeController.getRoomType(req as Request, res as Response, next);

      expect(next).toHaveBeenCalledWith(error);
    });
  });

  describe("getRoomTypes", () => {
    it("should return a list of room types", async () => {
      const roomTypesDTO = [{ name: "Operating Room", id: "1" }, { name: "ICU", id: "2" }];
      roomTypeServiceMock.getRoomTypes.mockResolvedValue(Result.ok(roomTypesDTO));

      await roomTypeController.getRoomTypes(req as Request, res as Response, next);

      expect(res.status).toHaveBeenCalledWith(200);
      expect(res.json).toHaveBeenCalledWith(roomTypesDTO);
    });

    it("should return 400 on failure to get room types", async () => {
      roomTypeServiceMock.getRoomTypes.mockResolvedValue(Result.fail("Failed to get room types"));

      await roomTypeController.getRoomTypes(req as Request, res as Response, next);

      expect(res.status).toHaveBeenCalledWith(400);
      expect(res.json).toHaveBeenCalledWith({ message: "Failed to get room types" });
    });

    it("should call next on error", async () => {
      const error = new Error("Unexpected error");
      roomTypeServiceMock.getRoomTypes.mockRejectedValue(error);

      await roomTypeController.getRoomTypes(req as Request, res as Response, next);

      expect(next).toHaveBeenCalledWith(error);
    });
  });
});
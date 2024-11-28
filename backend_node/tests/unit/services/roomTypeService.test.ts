import { Result } from "../../../src/core/logic/Result";
import { RoomType } from "../../../src/domain/roomType/roomType"; 
import IRoomTypeDTO from "../../../src/dto/IRoomTypeDTO";
import { RoomTypeMap } from "../../../src/mappers/RoomTypeMap";
import IRoomTypeRepo from "../../../src/services/IRepos/IRoomTypeRepo";
import RoomTypeService from "../../../src/services/roomTypeService"; 
import { describe, it, expect, jest, beforeEach } from '@jest/globals';

jest.mock("../../../src/mappers/RoomTypeMap");
jest.mock("../../../src/services/IRepos/IRoomTypeRepo");

describe("RoomTypeService", () => {
  let roomTypeService: RoomTypeService;
  let roomTypeRepoMock: jest.Mocked<IRoomTypeRepo>;

  beforeEach(() => {
    roomTypeRepoMock = {
      findByDomainId: jest.fn(),
      save: jest.fn(),
      findAll: jest.fn(),
      exists: jest.fn(),  
    };

    roomTypeService = new RoomTypeService(roomTypeRepoMock); 
  });

  describe("addRoomType", () => {
    it("should add a room type successfully", async () => {

      const roomTypeDTO: IRoomTypeDTO = { name: "Operating Room", id: "1" };

      const roomType = RoomType.create(roomTypeDTO);
      
      roomTypeRepoMock.save.mockResolvedValue(roomType.getValue()); 

      (RoomTypeMap.toDTO as jest.Mock).mockReturnValue(roomTypeDTO);

      const result = await roomTypeService.addRoomType(roomTypeDTO);

      expect(result.isSuccess).toBe(true);
      expect(result.getValue()).toEqual(roomTypeDTO);
      expect(roomTypeRepoMock.save).toHaveBeenCalledTimes(1);
    });

    it("should return a failure if RoomType creation fails", async () => {
      const roomTypeDTO: IRoomTypeDTO = { name: "", id: "1" };

      const roomTypeResult: Result<RoomType> = Result.fail('Room type name cannot be empty');
      (RoomType.create as jest.Mock).mockReturnValue(roomTypeResult);

      const result = await roomTypeService.addRoomType(roomTypeDTO);

      expect(result.isFailure).toBe(true);
      expect(result.errorValue()).toBe("Room type name cannot be empty");
    });
  });

  describe("getRoomType", () => {
    it("should return a room type by ID", async () => {
      const roomTypeDTO: IRoomTypeDTO = { name: "Operating Room", id: "1" };
      const roomType = RoomType.create(roomTypeDTO); 

      roomTypeRepoMock.findByDomainId.mockResolvedValue(roomType.getValue()); 

      (RoomTypeMap.toDTO as jest.Mock).mockReturnValue(roomTypeDTO);

      const result = await roomTypeService.getRoomType("1");

      expect(result.isSuccess).toBe(true);
      expect(result.getValue()).toEqual(roomTypeDTO);
    });

    it("should return a failure if room type is not found", async () => {
    
        roomTypeRepoMock.findByDomainId.mockResolvedValue(null as unknown as RoomType);
    
        const result = await roomTypeService.getRoomType("non-existing-id");
    
        expect(result.isFailure).toBe(true);
        expect(result.errorValue()).toBe("Room Type not found");
      });
  });

  describe("getRoomTypes", () => {
    it("should return a list of room types", async () => {
        const roomTypesDTO: IRoomTypeDTO[] = [
          { name: "Operating Room", id: "1" },
          { name: "ICU", id: "2" },
        ];
    
        const roomTypes = roomTypesDTO.map(dto => RoomType.create(dto).getValue());
    
        roomTypeRepoMock.findAll.mockResolvedValue(roomTypes);
    
        jest.spyOn(RoomTypeMap, "toDTO").mockImplementation((roomType: RoomType) => ({
            id: roomType.id.toString(),
            name: roomType.name.value,
        }));
    
        const result = await roomTypeService.getRoomTypes();
    
        expect(result.isSuccess).toBe(true);
        expect(result.getValue()).toEqual(roomTypesDTO);
        expect(roomTypeRepoMock.findAll).toHaveBeenCalledTimes(1);
      });

    it("should return a failure if there is an error fetching room types", async () => {
      roomTypeRepoMock.findAll.mockRejectedValue(new Error("Database error")); 

      const result = await roomTypeService.getRoomTypes();

      expect(result.isFailure).toBe(true);
      expect(result.errorValue()).toBe("Database error");
    });
  });
});
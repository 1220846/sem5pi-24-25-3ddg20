import { RoomType } from "../../../../src/domain/roomType/roomType"; 
import { UniqueEntityID } from "../../../../src/core/domain/UniqueEntityID";
import IRoomTypeDTO from "../../../../src/dto/IRoomTypeDTO";
import { describe, it, expect } from '@jest/globals';

describe('RoomType', () => {

    it('should create a RoomType successfully with valid data', () => {
        const roomTypeDTO: IRoomTypeDTO = { name: 'Operating Room' };  

        const result = RoomType.create(roomTypeDTO);

        expect(result.isSuccess).toBe(true); 
        expect(result.getValue()).toBeInstanceOf(RoomType);  
        expect(result.getValue().name.value).toBe('Operating Room');
    });

    it('should fail when name is invalid', () => {
        const invalidRoomTypeDTO: IRoomTypeDTO = { name: '' };  

        const result = RoomType.create(invalidRoomTypeDTO);

        expect(result.isFailure).toBe(true);  
        expect(result.errorValue()).toBe('Room type name cannot be empty'); 
    });

    it('should fail when name is null or undefined', () => {
        const nullRoomTypeDTO: IRoomTypeDTO = { name: null as unknown as string };  
        const undefinedRoomTypeDTO: IRoomTypeDTO = { name: undefined as unknown as string };  

        const resultNull = RoomType.create(nullRoomTypeDTO);
        const resultUndefined = RoomType.create(undefinedRoomTypeDTO);

        expect(resultNull.isFailure).toBe(true); 
        expect(resultNull.errorValue()).toBe('roomTypeName is null or undefined');  
 
        expect(resultUndefined.isFailure).toBe(true);
        expect(resultUndefined.errorValue()).toBe('roomTypeName is null or undefined');  
    });

    it('should create RoomType with a provided ID', () => {
        const roomTypeDTO: IRoomTypeDTO = { name: 'Operating Room' };
        const id = new UniqueEntityID();  

        const result = RoomType.create(roomTypeDTO, id);

        expect(result.isSuccess).toBe(true);  
        expect(result.getValue().id).toEqual(id);  
    });

    it('should create RoomType without an ID and generate one automatically', () => {
        const roomTypeDTO: IRoomTypeDTO = { name: 'Operating Room' };

        const result = RoomType.create(roomTypeDTO); 

        expect(result.isSuccess).toBe(true);  
        expect(result.getValue().id).not.toBeNull(); 
    });

});

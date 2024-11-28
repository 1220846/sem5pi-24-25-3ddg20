import { RoomTypeName } from "../../../../src/domain/roomType/roomTypeName"; 
import { describe, it, expect } from '@jest/globals';

describe('RoomTypeName', () => {
  
  it('should create a valid RoomTypeName', () => {
    const roomTypeName = 'Operating Room';
    const result = RoomTypeName.create(roomTypeName);

    expect(result.isSuccess).toBe(true);
    expect(result.getValue().value).toBe(roomTypeName);
  });

  it('should fail when roomTypeName is null or undefined', () => {
    const result = RoomTypeName.create(null as unknown as string);  
    expect(result.isFailure).toBe(true);
    expect(result.errorValue()).toBe('roomTypeName is null or undefined');

    const resultUndefined = RoomTypeName.create(undefined as unknown as string);  
    expect(resultUndefined.isFailure).toBe(true);
    expect(resultUndefined.errorValue()).toBe('roomTypeName is null or undefined');
  });

  it('should fail when roomTypeName is an empty string', () => {
    const result = RoomTypeName.create('');
    expect(result.isFailure).toBe(true);
    expect(result.errorValue()).toBe('Room type name cannot be empty');
  });

  it('should fail when roomTypeName has a length of 0', () => {
    const result = RoomTypeName.create(' ');
    expect(result.isFailure).toBe(true);
    expect(result.errorValue()).toBe('Room type name cannot be empty');
  });
});

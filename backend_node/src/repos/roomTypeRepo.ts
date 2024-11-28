import { Service, Inject } from 'typedi';
import IRoomTypeRepo from '../services/IRepos/IRoomTypeRepo';
import { Document, FilterQuery, Model } from 'mongoose';
import { IRoomTypePersistence } from '../dataschema/IRoomTypePersistence';
import { RoomType } from '../domain/roomType/roomType';
import { RoomTypeId } from '../domain/roomType/roomTypeId';
import { RoomTypeMap } from '../mappers/RoomTypeMap';

@Service()
export default class RoomTypeRepo implements IRoomTypeRepo {
    private models: any;

    constructor(
        @Inject('roomTypeSchema') private roomTypeSchema: Model<IRoomTypePersistence & Document>,
    ) { }

    private createBaseQuery(): any {
        return {
            where: {},
        }
    }

    public async exists(roomType: RoomType): Promise<boolean> {

        const idX = roomType.id instanceof RoomTypeId ? (<RoomTypeId>roomType.id).toValue() : roomType.id;

        const query = { domainId: idX };
        const roomTypeDocument = await this.roomTypeSchema.findOne(query as FilterQuery<IRoomTypePersistence & Document>);

        return !!roomTypeDocument === true;
    }

    public async save(roomType: RoomType): Promise<RoomType> {
        const query = { domainId: roomType.id.toString() };

        const roomTypeDocument = await this.roomTypeSchema.findOne(query);

        try {
            if (roomTypeDocument === null) {
                const rawRoomType: any = RoomTypeMap.toPersistence(roomType);

                const roomTypeCreated = await this.roomTypeSchema.create(rawRoomType);

                return RoomTypeMap.toDomain(roomTypeCreated);
            } else {
                roomTypeDocument.name = roomType.name.value;
                await roomTypeDocument.save();

                return roomType;
            }
        } catch (err) {
            throw err;
        }
    }

    public async findByDomainId(roomTypeId: RoomTypeId | string): Promise<RoomType> {
        const query = { domainId: roomTypeId };
        const roomTypeRecord = await this.roomTypeSchema.findOne(query as FilterQuery<IRoomTypePersistence & Document>);

        if (roomTypeRecord != null) {
            return RoomTypeMap.toDomain(roomTypeRecord);
        }
        else
            return null;
    }

    public async findAll(): Promise<RoomType[]> {
        
        const roomTypeDocuments = await this.roomTypeSchema.find();
        
        const roomTypes: RoomType[] = [];
        for (const roomTypeDocument of roomTypeDocuments) {
            const roomType = await RoomTypeMap.toDomain(roomTypeDocument);

            if (roomType) {
                roomTypes.push(roomType);
            }
        }
        
        return roomTypes;
    }
    
}
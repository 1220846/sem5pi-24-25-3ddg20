import mongoose from "mongoose";
import { IRoomTypePersistence } from "../../dataschema/IRoomTypePersistence";

const RoomTypeSchema = new mongoose.Schema(
    {
        domainId:{type: String, unique: true},
        name: {type: String, unique: true}
    },
    {
        timestamps: true
    }
)

export default mongoose.model<IRoomTypePersistence & mongoose.Document>('RoomType',RoomTypeSchema);
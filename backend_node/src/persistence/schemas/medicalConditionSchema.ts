import mongoose from "mongoose";
import { IMedicalConditionPersistence } from "../../dataschema/IMedicalConditionPersistence";

const MedicalConditionSchema = new mongoose.Schema(
    {
        domainId:{type: String, unique: true},
        code: {type: String, unique: true},
        designation: {type: String, unique: true},
        description: {type: String},
    },
    {
        timestamps: true
    }
)
export default mongoose.model<IMedicalConditionPersistence & mongoose.Document>('MedicalCondition',MedicalConditionSchema);
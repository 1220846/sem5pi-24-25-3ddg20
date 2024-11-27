import mongoose from 'mongoose';
import { Db } from 'mongodb';
import config from '../../config';

mongoose.set('strictQuery', true); // Set this explicitly to avoid warnings

export default async (): Promise<Db> => {
  try {
    const connection = await mongoose.connect(config.databaseURL, {
      useNewUrlParser: true, // (Optional: Remove if using latest drivers)
      useUnifiedTopology: true, // (Optional: Remove if using latest drivers)
    });
    console.log('MongoDB connected to', connection.connection.host);
    return connection.connection.db;
  } catch (error) {
    console.error('MongoDB connection error:', error);
    throw error;
  }
};
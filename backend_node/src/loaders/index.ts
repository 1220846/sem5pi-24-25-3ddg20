import expressLoader from './express';
import dependencyInjectorLoader from './dependencyInjector';
import mongooseLoader from './mongoose';
import Logger from './logger';

import config from '../../config';

export default async ({ expressApp }) => {
  const mongoConnection = await mongooseLoader();
  Logger.info('✌️ DB loaded and connected!');

  const userSchema = {
    // compare with the approach followed in repos and services
    name: 'userSchema',
    schema: '../persistence/schemas/userSchema',
  };

  const roleSchema = {
    // compare with the approach followed in repos and services
    name: 'roleSchema',
    schema: '../persistence/schemas/roleSchema',
  };

  const roomTypeSchema = {
    // compare with the approach followed in repos and services
    name: 'roomTypeSchema',
    schema: '../persistence/schemas/roomTypeSchema',
  };

  const roleController = {
    name: config.controllers.role.name,
    path: config.controllers.role.path
  }

  const roleRepo = {
    name: config.repos.role.name,
    path: config.repos.role.path
  }

  const userRepo = {
    name: config.repos.user.name,
    path: config.repos.user.path
  }

  const roleService = {
    name: config.services.role.name,
    path: config.services.role.path
  }

  // RoomType
  const roomTypeController = {
    name: config.controllers.roomType.name,
    path: config.controllers.roomType.path
  }

  const roomTypeRepo = {
    name: config.repos.roomType.name,
    path: config.repos.roomType.path
  }

  const roomTypeService = {
    name: config.services.roomType.name,
    path: config.services.roomType.path
  }

  await dependencyInjectorLoader({
    mongoConnection,
    schemas: [
      userSchema,
      roleSchema,
      roomTypeSchema
    ],
    controllers: [
      roleController,
      roomTypeController
    ],
    repos: [
      roleRepo,
      userRepo,
      roomTypeRepo
    ],
    services: [
      roleService,
      roomTypeService
    ]
  });
  Logger.info('✌️ Schemas, Controllers, Repositories, Services, etc. loaded');

  await expressLoader({ app: expressApp });
  Logger.info('✌️ Express loaded');
};

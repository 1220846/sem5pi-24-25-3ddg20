import { Router } from 'express';
import auth from './routes/userRoute';
import user from './routes/userRoute';
import role from './routes/roleRoute';
import roomType from './routes/roomTypeRoute';

export default () => {
	const app = Router();

	auth(app);
	user(app);
	role(app);
	roomType(app);
	
	return app
}
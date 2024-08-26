import { isRejectedWithValue } from '@reduxjs/toolkit';

/**
 * Log a warning and show a toast!
 */
const rtkQueryErrorLogger = () => (next) => (action) => {
	// RTK Query uses `createAsyncThunk` from redux-toolkit under the hood, so we're able to utilize these matchers!
	if (isRejectedWithValue(action)) {
		if (action.payload.status === 401 && action.payload.data.errorMessage === 'unauthenticated') {
			return next(action);
		}

		if (action.payload.status === 401 && action.payload.data.errorMessage === 'unauthorized') {
			window.location.href = '/unauthorized';
		}

		// showNotification({
		// 	title: 'Error',
		// 	message: action.payload.data.errorMessage,
		// 	color: 'red',
		// 	icon: <IconExclamationMark />,
		// });

		console.warn('We got a rejected action!', action);
	}

	return next(action);
};

export default rtkQueryErrorLogger;

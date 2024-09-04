import * as yup from 'yup';

const defaultValues = {
	amount: '',
	description: '',
	currency: '',
	transactionType: '',
	categoryId: '',
	userId: '',
	date: '',
	isRecurrent: false,
};

const schema = yup.object().shape({
	amount: yup
		.number()
		.positive('This field can contain only positive numbers')
		.typeError('This field can contain only numbers')
		.required('Amount is a required field'),
	description: yup.string().required('Description is a required field'),
	currency: yup.string().required('Currency is a required field'),
	transactionType: yup.string().required('Transaction type is a required field'),
	categoryId: yup.string().required('Category is a required field'),
	userId: yup.string().required('User is a required field'),
	date: yup.date().required('Date is a required field'),
	isRecurrent: yup.bool().required('Recurrent is a required field'),
});

export { defaultValues, schema };

import React, { useState } from 'react';
import { Button, Chip, Fieldset, Group, Overlay, Select, Stack, TextInput } from '@mantine/core';
import { DateTimePicker } from '@mantine/dates';
import { useForm } from 'react-hook-form';
import { useCreateTransactionMutation } from '../../../../state/transactions/api';
import Swal from 'sweetalert2';

const CreateTransactionOverlay = ({ id, close }) => {
	const categories = [
		{
			Id: 'a14c46ac-222e-4de9-8a00-158bc1d3d5c2',
			Name: 'Groceries',
			CategoryType: 6, // Expense
		},
		{
			Id: '1b0dc89f-8751-43b9-82f7-1ea1447c4e58',
			Name: 'Utilities',
			CategoryType: 6, // Expense
		},
		{
			Id: '3651bef6-49b3-4f31-9fa7-289263ce3584',
			Name: 'Internet',
			CategoryType: 6, // Expense
		},
		{
			Id: 'd3a1c01f-a35c-488a-b121-2f266ba83bea',
			Name: 'nmew cat',
			CategoryType: 6, // Expense
		},
		{
			Id: 'bc90050b-5455-42d7-bc2e-32e93c3149b1',
			Name: 'Rent',
			CategoryType: 6, // Expense
		},
		{
			Id: '11f3afef-c30f-4e73-9daa-36a9ff1c2e31',
			Name: 'Transportation',
			CategoryType: 6, // Expense
		},
		{
			Id: 'd2d5d78a-49a7-4d4d-a226-50461653bbbb',
			Name: 'Fuel',
			CategoryType: 6, // Expense
		},
		{
			Id: 'af76f58e-0de3-4299-98b2-82beb87e9624',
			Name: 'Car Maintenance',
			CategoryType: 6, // Expense
		},
		{
			Id: '1e6c606f-192a-49cd-a255-ab87323f2e80',
			Name: 'Bus',
			CategoryType: 6, // Expense
		},
		{
			Id: '3d848062-7e1c-43ff-a5d7-b5adff2f892c',
			Name: 'Electricity',
			CategoryType: 6, // Expense
		},
		{
			Id: '1ec53d35-9e02-4c7c-9e25-b6dbac0cbec3',
			Name: 'new category',
			CategoryType: 6, // Expense
		},
		{
			Id: '370b4b14-0bad-4a37-96ff-f2d4e5a1aa5a',
			Name: 'Gas',
			CategoryType: 6, // Expense
		},
	];
	const transactionTypes = [
		{
			Id: 1,
			Name: 'Income',
		},
		{
			Id: 2,
			Name: 'Deposit',
		},
		{
			Id: 3,
			Name: 'Refund',
		},
		{
			Id: 4,
			Name: 'Investment',
		},
		{
			Id: 5,
			Name: 'Withdrawal',
		},
		{
			Id: 6,
			Name: 'Expense',
		},
		{
			Id: 7,
			Name: 'Transfer',
		},
	];

	const [categoryId, setCategoryId] = useState();
	const [transactionType, setTransactionType] = useState();
	const [date, setDate] = useState('2024-08-31T07:14:58.004Z');

	const [createTransaction, createTransactionResult] = useCreateTransactionMutation();

	const { register, handleSubmit } = useForm();
	const onSubmit = async (data) => {
		try {
			const body = {
				description: data.description,
				amount: data.amount,
				date: new Date(date).toISOString(),
				categoryId: categoryId,
				isRecurrent: data.isRecurrent,
				transactionType: transactionType,
				userId: id,
			};

			await createTransaction(body);

			Swal.fire({
				title: 'Tranzactie adaugata !',
				text: 'Tranzactia a fost adaugata cu succes !',
				icon: 'success',
			}).then(() => close());
		} catch (err) {
			Swal.fire({
				title: 'Oops...',
				text: 'A aparut o eroare',
				icon: 'error',
			});
		}
	};
	return (
		<Overlay color='#000' backgroundOpacity={0.35} blur={7}>
			<div
				style={{
					padding: '1.25rem',
					borderRadius: '1rem',
					backgroundColor: 'white',
					position: 'absolute',
					top: '50%',
					left: '50%',
					transform: 'translate(-50%, -50%)',
				}}
			>
				<Fieldset legend='Register a new transaction' p={'lg'} radius={'lg'}>
					<form onSubmit={handleSubmit(onSubmit)} action=''>
						<Group align='center' justify='center'>
							<Stack>
								<TextInput {...register('description')} label='Description' placeholder='Car fuel' />
								<TextInput {...register('amount')} label='Transaction amount' placeholder='450' mt='md' />
							</Stack>
							<Stack>
								<Select
									name='transactionType'
									// {...register('transactionType')}
									onChange={(e) => setTransactionType(e)}
									searchable
									label='Type of the transaction'
									placeholder='Pick value'
									data={transactionTypes.map((c) => ({ value: c.Id.toString(), label: c.Name }))}
								/>
								<Select
									// {...register('categoryId')}
									onChange={(e) => setCategoryId(e)}
									name='categoryId'
									mt={'md'}
									searchable
									label='Category of the transaction'
									placeholder='Pick value'
									data={categories.map((c) => ({ value: c.Id, label: c.Name }))}
								/>
							</Stack>
							<Stack>
								<DateTimePicker
									// {...register('date')}
									onChange={(e) => setDate(new Date(e).toLocaleString('en-US'))}
									label='Transaction Date'
									placeholder={new Date().toLocaleString('es-CL')}
								/>{' '}
								<Chip {...register('isRecurrent')} mt={'xl'}>
									Recurent Transaction
								</Chip>{' '}
							</Stack>
						</Group>
						<Button type='submit' fullWidth mt={'md'}>
							Create
						</Button>
					</form>
				</Fieldset>
			</div>
		</Overlay>
	);
};

export default CreateTransactionOverlay;

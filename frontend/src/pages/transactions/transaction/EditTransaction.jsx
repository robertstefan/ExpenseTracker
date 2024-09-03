import React from 'react';
import { Button, NumberInput, Select, Switch, TextInput, Title } from '@mantine/core';
import { Controller, useForm } from 'react-hook-form';
import { useParams } from 'react-router-dom';
import { notifications } from '@mantine/notifications';
import { DateInput } from '@mantine/dates';
import dayjs from 'dayjs';
import { currencies, transactionTypes } from '../../../state/constants';
import { useUpdateTransactionMutation } from '../../../state/transaction/api';
import { useGetCategoriesQuery } from '../../../state/category/api';
import { useGetUsersQuery } from '../../../state/user/api';

const EditTransaction = () => {
	const { id } = useParams();

	const [updateTransaction, resultUpdateTransaction] = useUpdateTransactionMutation();
	const { data: categories = [] } = useGetCategoriesQuery();
	const { data: users = [] } = useGetUsersQuery();

	const {
		control,
		register,
		handleSubmit,
		formState: { errors },
	} = useForm();

	const onSubmit = async (data) => {
		console.log({ data });
		await updateTransaction({ ...data, id, date: dayjs(data.date).format('YYYY-MM-DD') });

		notifications.show({
			title: 'Transaction modified',
			position: 'bottom-right',
		});

		// navigate('/category');
	};

	console.log({ categories, users });
	return (
		<div>
			<Title>New Category</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<NumberInput {...register('amount')} label='Amount' withAsterisk />
				<TextInput {...register('description', { required: 'Description is a required field!' })} label='Description' withAsterisk />
				<Controller
					name='currency'
					control={control}
					render={({ field }) => <Select {...field} label='Currency' placeholder='Pick a currency' data={currencies} />}
				/>
				<Controller
					name='transactionType'
					control={control}
					render={({ field }) => <Select {...field} label='Type' placeholder='Pick a transaction type' data={transactionTypes} />}
				/>
				<Controller
					name='categoryId'
					control={control}
					render={({ field }) => (
						<Select
							{...field}
							label='Category'
							placeholder='Pick a category'
							data={categories.map((category) => ({ value: category.id, label: category.name }))}
						/>
					)}
				/>
				<Controller
					name='userId'
					control={control}
					render={({ field }) => (
						<Select
							{...field}
							label='User'
							placeholder='Pick an user'
							data={users.map((user) => ({ value: `${user.id}`, label: `${user.firstName} ${user.lastName}` }))}
						/>
					)}
				/>
				<Controller
					name='date'
					control={control}
					render={({ field }) => <DateInput {...field} valueFormat='DD/MM/YYYY' label='Date' placeholder='Date' />}
				/>
				<Controller name='isRecurrent' control={control} render={({ field }) => <Switch {...field} label='Recurrent' />} />
				<Button type='submit' mt='md' disabled={resultUpdateTransaction?.isLoading}>
					Submit
				</Button>
			</form>
		</div>
	);
};

export default EditTransaction;

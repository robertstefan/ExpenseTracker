import React, { useEffect } from 'react';
import { Button, NumberInput, Select, Switch, TextInput, Title } from '@mantine/core';
import { Controller, useForm } from 'react-hook-form';
import { useParams } from 'react-router-dom';
import { notifications } from '@mantine/notifications';
import { DateInput } from '@mantine/dates';
import { yupResolver } from '@hookform/resolvers/yup';
import dayjs from 'dayjs';
import { currencies, transactionTypes } from '../../../state/constants';
import { useGetTransactionQuery, useUpdateTransactionMutation } from '../../../state/transaction/api';
import { useGetCategoriesQuery } from '../../../state/category/api';
import { useGetUsersQuery } from '../../../state/user/api';
import { defaultValues, schema } from './EditTransaction.logic';

const EditTransaction = () => {
	const { id } = useParams();

	const { data: transaction } = useGetTransactionQuery(id);
	const [updateTransaction, resultUpdateTransaction] = useUpdateTransactionMutation();
	const { data: categories = [] } = useGetCategoriesQuery();
	const { data: users = [] } = useGetUsersQuery();

	const {
		control,
		register,
		handleSubmit,
		reset,
		formState: { errors },
	} = useForm({ defaultValues, resolver: yupResolver(schema) });

	useEffect(() => {
		if (transaction) {
			reset({
				amount: transaction.amount,
				description: transaction.description,
				currency: transaction.currency,
				transactionType: transaction.transactionType,
				categoryId: transaction.categoryId,
				userId: transaction.userId,
				date: dayjs(transaction.date, 'YYYY-MM-DDTHH:mm:ss').toDate(),
				isRecurrent: transaction.isRecurrent,
			});
		}
	}, [transaction]);

	const onSubmit = async (data) => {
		await updateTransaction({ ...data, id, date: dayjs(data.date).format('YYYY-MM-DD') });

		notifications.show({
			title: 'Transaction modified',
			position: 'bottom-right',
		});
	};

	return (
		<div>
			<Title>Edit Transaction</Title>
			<form onSubmit={handleSubmit(onSubmit)}>
				<NumberInput {...register('amount')} label='Amount' withAsterisk error={errors.amount?.message} />
				<TextInput
					{...register('description', { required: 'Description is a required field!' })}
					label='Description'
					withAsterisk
					error={errors.description?.message}
				/>
				<Controller
					name='currency'
					control={control}
					render={({ field }) => (
						<Select {...field} label='Currency' placeholder='Pick a currency' data={currencies} error={errors.currency?.message} />
					)}
				/>
				<Controller
					name='transactionType'
					control={control}
					render={({ field }) => (
						<Select
							{...field}
							label='Type'
							placeholder='Pick a transaction type'
							data={transactionTypes}
							error={errors.transactionType?.message}
						/>
					)}
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
							error={errors.categoryId?.message}
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
							error={errors.userId?.message}
						/>
					)}
				/>
				<Controller
					name='date'
					control={control}
					render={({ field }) => (
						<DateInput {...field} valueFormat='DD/MM/YYYY' label='Date' placeholder='Date' error={errors.date?.message} />
					)}
				/>
				<Controller
					name='isRecurrent'
					control={control}
					render={({ field }) => <Switch {...field} label='Recurrent' error={errors.isRecurrent?.message} />}
				/>
				<Button type='submit' mt='md' disabled={resultUpdateTransaction?.isLoading}>
					Submit
				</Button>
			</form>
		</div>
	);
};

export default EditTransaction;

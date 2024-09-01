import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { Button, Group, TextInput, Title } from '@mantine/core';
import { notifications } from '@mantine/notifications';
import { useDeleteCategoryMutation, useGetCategoryQuery, useUpdateCategoryMutation } from '../../../state/category/api';

const EditCategory = () => {
    const { id } = useParams();
    const navigate = useNavigate();

    // Fetch the existing category data
    const { data: category, isLoading: isFetching } = useGetCategoryQuery(id);

    // Hook for handling form state and submission
    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors },
    } = useForm();

    // Mutations for deleting and updating the category
    const [deleteCategory] = useDeleteCategoryMutation();
    const [updateCategory, { isLoading: isUpdating }] = useUpdateCategoryMutation();

    // Pre-fill the form with the category data when it's fetched
    useEffect(() => {
        if (category) {
            setValue('name', category.name);
        }
    }, [category, setValue]);

    // Handle form submission to update the category
    const onSubmit = async (data) => {
        // Construct the full category object
        const updatedCategory = { id, name: data.name };
        await updateCategory(updatedCategory);
        notifications.show({
            title: 'Category Updated',
            message: `Category ${data.name} was updated!`,
            position: 'bottom-right',
        });
        navigate('/category');
    };

    // Handle category deletion
    const handleDelete = async () => {
        await deleteCategory(id);
        notifications.show({
            title: 'Category Deleted',
            message: `Category ${category.name} was deleted!`,
            position: 'bottom-right',
        });
        navigate('/category');
    };

    // Show a loading state while fetching the category data
    if (isFetching) return <div>Loading...</div>;

    return (
        <div>
            <Group justify='space-between'>
                <Title>Edit Category</Title>
                <Button color="red" onClick={handleDelete} disabled={isUpdating}>
                    Delete
                </Button>
            </Group>
            <form onSubmit={handleSubmit(onSubmit)}>
                <TextInput
                    {...register('name', { required: 'Category name is a required field!' })}
                    label='Name'
                    withAsterisk
                    error={errors.name?.message}
                />
                <Button type='submit' mt='md' disabled={isUpdating}>
                    Update
                </Button>
            </form>
        </div>
    );
};

export default EditCategory;

import { createTheme } from '@mantine/core';
import { IconCalendar } from '@tabler/icons-react';

const components = {
	Loader: {
		defaultProps: {
			variant: 'bars',
		},
	},
	Menu: {
		defaultProps: {
			withArrow: true,
			shadow: 'md',
			radius: 'md',
		},
	},
	Paper: {
		defaultProps: {
			shadow: 'sm',
			radius: 'lg',
			p: 'md',
		},
	},
	Modal: {
		defaultProps: {
			padding: 'xl',
			centered: true,
		},
		styles: () => ({
			content: {
				padding: '0 !important',
			},
		}),
	},
	Button: {
		defaultProps: {
			size: 'sm',
			color: 'cindigo',
			radius: 'xl',
			fontFamily: 'ProximaNova-Semibold',
		},
	},
	Checkbox: {
		defaultProps: {
			color: 'cindigo',
		},
	},
	Tabs: {
		defaultProps: {
			color: 'cindigo',
			variant: 'pills',
			radius: 'md',
			keepMounted: false,
		},
	},
	ActionIcon: {
		defaultProps: {
			variant: 'subtle',
			radius: 'xl',
			color: 'cindigo',
		},
	},
	ThemeIcon: {
		defaultProps: {
			variant: 'filled',
			radius: 'xl',
			color: 'cindigo',
		},
	},
	Pagination: {
		defaultProps: {
			color: 'cindigo',
			withEdges: true,
		},
	},
	Switch: {
		defaultProps: {
			size: 'xs',
			onLabel: 'ON',
			offLabel: 'OFF',
		},
	},
	LoadingOverlay: {
		defaultProps: {
			// overlayBlur: 2,
		},
	},
	DatePickerInput: {
		defaultProps: {
			// overlayBlur: 2,
			valueFormat: 'DD-MM-YYYY',
			icon: <IconCalendar size='1.1rem' stroke={1.5} />,
		},
	},
};

const theme = createTheme({
	fontFamily: 'ProximaNova-Regular, sans-serif',
	colors: {
		cindigo: ['#E5EDFF', '#4C7EFF', '#4C7EFF', '#4C7EFF', '#4C7EFF', '#4C7EFF', '#4C7EFF', '#4C7EFF', '#4C7EFF', '#000E33'],
	},
	components,
	primaryColor: 'cindigo',
});

export default theme;

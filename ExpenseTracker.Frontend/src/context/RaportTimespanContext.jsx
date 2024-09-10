import { createContext, useContext, useState } from 'react';

const RaportTimespanContext = createContext(null);

export const useRaportTimespan = () => {
	const context = useContext(RaportTimespanContext);

	return context;
};

export const RaportTimespanProvider = ({ children }) => {
	const [raportTimespan, setRaportTimespan] = useState(1);
	const [day, setDay] = useState(null);
	const [week, setWeek] = useState(null);
	const [month, setMonth] = useState(null);
	const [year, setYear] = useState(null);
	const [raportPeriod, setRaportPeriod] = useState(null);

	const [timeRange, setTimeRange] = useState(null);
	return (
		<RaportTimespanContext.Provider
			value={{
				raportTimespan,
				setRaportTimespan,
				day,
				setDay,
				week,
				setWeek,
				month,
				setMonth,
				year,
				setYear,
				raportPeriod,
				setRaportPeriod,
			}}
		>
			{children}
		</RaportTimespanContext.Provider>
	);
};

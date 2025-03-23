--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.2

-- Started on 2025-03-23 03:57:20

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 228 (class 1259 OID 16470)
-- Name: Bookings; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Bookings" (
    "Id" integer NOT NULL,
    "SailorId" integer NOT NULL,
    "DockingSpotId" integer NOT NULL,
    "StartDate" date NOT NULL,
    "EndDate" date NOT NULL,
    "PaymentMethodId" integer NOT NULL,
    "IsPaid" boolean DEFAULT false NOT NULL,
    "People" integer NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public."Bookings" OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 16537)
-- Name: Comments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Comments" (
    "Id" integer NOT NULL,
    "CreatedBy" integer,
    "GuideId" integer,
    "Content" text NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public."Comments" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16428)
-- Name: DockingSpots; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."DockingSpots" (
    "Id" integer NOT NULL,
    "Name" character varying(100),
    "Location" jsonb NOT NULL,
    "Description" text,
    "OwnerId" integer NOT NULL,
    "PortId" integer NOT NULL,
    "PricePerNight" double precision,
    "PricePerPerson" double precision,
    "IsAvailable" boolean DEFAULT true NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public."DockingSpots" OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 16501)
-- Name: Guides; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Guides" (
    "Id" integer NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Content" text NOT NULL,
    "CreatedBy" integer,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "Location" jsonb NOT NULL,
    "IsApproved" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Guides" OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 16517)
-- Name: Images; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Images" (
    "Id" integer NOT NULL,
    "Url" text NOT NULL,
    "CreatedBy" integer,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "GuideId" integer
);


ALTER TABLE public."Images" OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 16572)
-- Name: Notifications; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Notifications" (
    "Id" integer NOT NULL,
    "CreatedBy" integer,
    "Message" text NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public."Notifications" OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 16489)
-- Name: PaymentMethods; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."PaymentMethods" (
    "Id" integer NOT NULL,
    "Name" character varying(20) NOT NULL
);


ALTER TABLE public."PaymentMethods" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16412)
-- Name: Ports; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Ports" (
    "Id" integer NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "Name" character varying(255) NOT NULL,
    "Location" jsonb NOT NULL,
    "Description" text,
    "OwnerId" integer NOT NULL,
    "IsApproved" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Ports" OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 16557)
-- Name: Reviews; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Reviews" (
    "Id" integer NOT NULL,
    "CreatedBy" integer,
    "Rating" double precision NOT NULL,
    "Comment" text,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "PortId" integer NOT NULL
);


ALTER TABLE public."Reviews" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16398)
-- Name: Roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Roles" (
    "Id" integer NOT NULL,
    "Name" character varying(20) NOT NULL
);


ALTER TABLE public."Roles" OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16449)
-- Name: Services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Services" (
    "Id" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Description" text,
    "Price" numeric(10,2) NOT NULL,
    "PortId" integer,
    "DockingSpotId" integer,
    "IsAvailable" boolean DEFAULT true NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public."Services" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16388)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" integer NOT NULL,
    "CreatedOn" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "Name" character varying(50) NOT NULL,
    "Surname" character varying(50) NOT NULL,
    "Email" character varying(50) NOT NULL,
    "PhoneNumber" character varying(20),
    "Password" character varying(255) NOT NULL,
    "RoleId" integer
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 16469)
-- Name: bookings_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.bookings_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.bookings_id_seq OWNER TO postgres;

--
-- TOC entry 5044 (class 0 OID 0)
-- Dependencies: 227
-- Name: bookings_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.bookings_id_seq OWNED BY public."Bookings"."Id";


--
-- TOC entry 235 (class 1259 OID 16536)
-- Name: comments_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.comments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.comments_id_seq OWNER TO postgres;

--
-- TOC entry 5045 (class 0 OID 0)
-- Dependencies: 235
-- Name: comments_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.comments_id_seq OWNED BY public."Comments"."Id";


--
-- TOC entry 223 (class 1259 OID 16427)
-- Name: dockingspots_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dockingspots_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dockingspots_id_seq OWNER TO postgres;

--
-- TOC entry 5046 (class 0 OID 0)
-- Dependencies: 223
-- Name: dockingspots_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dockingspots_id_seq OWNED BY public."DockingSpots"."Id";


--
-- TOC entry 231 (class 1259 OID 16500)
-- Name: guides_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.guides_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.guides_id_seq OWNER TO postgres;

--
-- TOC entry 5047 (class 0 OID 0)
-- Dependencies: 231
-- Name: guides_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.guides_id_seq OWNED BY public."Guides"."Id";


--
-- TOC entry 233 (class 1259 OID 16516)
-- Name: images_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.images_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.images_id_seq OWNER TO postgres;

--
-- TOC entry 5048 (class 0 OID 0)
-- Dependencies: 233
-- Name: images_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.images_id_seq OWNED BY public."Images"."Id";


--
-- TOC entry 239 (class 1259 OID 16571)
-- Name: notifications_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.notifications_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.notifications_id_seq OWNER TO postgres;

--
-- TOC entry 5049 (class 0 OID 0)
-- Dependencies: 239
-- Name: notifications_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.notifications_id_seq OWNED BY public."Notifications"."Id";


--
-- TOC entry 229 (class 1259 OID 16488)
-- Name: paymentmethods_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.paymentmethods_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.paymentmethods_id_seq OWNER TO postgres;

--
-- TOC entry 5050 (class 0 OID 0)
-- Dependencies: 229
-- Name: paymentmethods_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.paymentmethods_id_seq OWNED BY public."PaymentMethods"."Id";


--
-- TOC entry 221 (class 1259 OID 16411)
-- Name: port_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.port_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.port_id_seq OWNER TO postgres;

--
-- TOC entry 5051 (class 0 OID 0)
-- Dependencies: 221
-- Name: port_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.port_id_seq OWNED BY public."Ports"."Id";


--
-- TOC entry 237 (class 1259 OID 16556)
-- Name: reviews_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reviews_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reviews_id_seq OWNER TO postgres;

--
-- TOC entry 5052 (class 0 OID 0)
-- Dependencies: 237
-- Name: reviews_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reviews_id_seq OWNED BY public."Reviews"."Id";


--
-- TOC entry 219 (class 1259 OID 16397)
-- Name: roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.roles_id_seq OWNER TO postgres;

--
-- TOC entry 5053 (class 0 OID 0)
-- Dependencies: 219
-- Name: roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.roles_id_seq OWNED BY public."Roles"."Id";


--
-- TOC entry 225 (class 1259 OID 16448)
-- Name: services_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.services_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.services_id_seq OWNER TO postgres;

--
-- TOC entry 5054 (class 0 OID 0)
-- Dependencies: 225
-- Name: services_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.services_id_seq OWNED BY public."Services"."Id";


--
-- TOC entry 217 (class 1259 OID 16387)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- TOC entry 5055 (class 0 OID 0)
-- Dependencies: 217
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public."Users"."Id";


--
-- TOC entry 4809 (class 2604 OID 16473)
-- Name: Bookings Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Bookings" ALTER COLUMN "Id" SET DEFAULT nextval('public.bookings_id_seq'::regclass);


--
-- TOC entry 4818 (class 2604 OID 16540)
-- Name: Comments Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Comments" ALTER COLUMN "Id" SET DEFAULT nextval('public.comments_id_seq'::regclass);


--
-- TOC entry 4803 (class 2604 OID 16431)
-- Name: DockingSpots Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DockingSpots" ALTER COLUMN "Id" SET DEFAULT nextval('public.dockingspots_id_seq'::regclass);


--
-- TOC entry 4813 (class 2604 OID 16504)
-- Name: Guides Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Guides" ALTER COLUMN "Id" SET DEFAULT nextval('public.guides_id_seq'::regclass);


--
-- TOC entry 4816 (class 2604 OID 16520)
-- Name: Images Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Images" ALTER COLUMN "Id" SET DEFAULT nextval('public.images_id_seq'::regclass);


--
-- TOC entry 4822 (class 2604 OID 16575)
-- Name: Notifications Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications" ALTER COLUMN "Id" SET DEFAULT nextval('public.notifications_id_seq'::regclass);


--
-- TOC entry 4812 (class 2604 OID 16492)
-- Name: PaymentMethods Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentMethods" ALTER COLUMN "Id" SET DEFAULT nextval('public.paymentmethods_id_seq'::regclass);


--
-- TOC entry 4800 (class 2604 OID 16415)
-- Name: Ports Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Ports" ALTER COLUMN "Id" SET DEFAULT nextval('public.port_id_seq'::regclass);


--
-- TOC entry 4820 (class 2604 OID 16560)
-- Name: Reviews Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reviews" ALTER COLUMN "Id" SET DEFAULT nextval('public.reviews_id_seq'::regclass);


--
-- TOC entry 4799 (class 2604 OID 16401)
-- Name: Roles Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Roles" ALTER COLUMN "Id" SET DEFAULT nextval('public.roles_id_seq'::regclass);


--
-- TOC entry 4806 (class 2604 OID 16452)
-- Name: Services Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services" ALTER COLUMN "Id" SET DEFAULT nextval('public.services_id_seq'::regclass);


--
-- TOC entry 4797 (class 2604 OID 16391)
-- Name: Users Id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN "Id" SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 5026 (class 0 OID 16470)
-- Dependencies: 228
-- Data for Name: Bookings; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Bookings" ("Id", "SailorId", "DockingSpotId", "StartDate", "EndDate", "PaymentMethodId", "IsPaid", "People", "CreatedOn") FROM stdin;
\.


--
-- TOC entry 5034 (class 0 OID 16537)
-- Dependencies: 236
-- Data for Name: Comments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Comments" ("Id", "CreatedBy", "GuideId", "Content", "CreatedOn") FROM stdin;
1	1	2	Zbs	2025-03-23 02:57:54.255923
2	2	1	Gowno, nie polecam	2025-03-23 02:57:54.255923
3	4	3	Really helpful guide!	2025-03-23 03:39:34.699544
4	5	2	Could use more details on docking.	2025-03-23 03:39:34.699544
5	6	5	Saved me from scraping my boat!	2025-03-23 03:39:34.699544
6	7	6	Funny and informative.	2025-03-23 03:39:34.699544
7	8	4	Needs images.	2025-03-23 03:39:34.699544
8	9	7	Great photo tips!	2025-03-23 03:39:34.699544
9	10	1	This one was a life-saver.	2025-03-23 03:39:34.699544
10	2	3	Pretty basic but useful.	2025-03-23 03:39:34.699544
\.


--
-- TOC entry 5022 (class 0 OID 16428)
-- Dependencies: 224
-- Data for Name: DockingSpots; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."DockingSpots" ("Id", "Name", "Location", "Description", "OwnerId", "PortId", "PricePerNight", "PricePerPerson", "IsAvailable", "CreatedOn") FROM stdin;
1	Sunset Dock	{"Town": "Bogaczewo", "Latitude": 53.963, "Longitude": 21.748}	Perfect for catching the sunset. Includes water & power hookup.	3	1	150	20	t	2025-03-23 02:46:05.359461
2	Budget Bay	{"Town": "Bogaczewo", "Latitude": 53.961, "Longitude": 21.746}	Affordable dock close to shore with basic facilities.	3	1	80	10	t	2025-03-23 02:46:05.359461
3	Main Marina Spot	{"Town": "Gizycko", "Latitude": 54.032, "Longitude": 21.768}	Centrally located dock with easy town access.	3	2	200	25	t	2025-03-23 02:46:05.359461
4	Quiet Corner	{"Town": "Gizycko", "Latitude": 54.030, "Longitude": 21.766}	Private spot away from the crowd, surrounded by trees.	3	2	120	15	f	2025-03-23 02:46:05.359461
5	Eastern Edge Dock	{"Town": "Bogaczewo", "Latitude": 53.9667789, "Longitude": 21.7483321}	Scenic spot ideal for sunrise views.	3	1	130	15	t	2025-03-23 03:36:55.214364
6	Lagoon Lounge	{"Town": "Bogaczewo", "Latitude": 53.9681221, "Longitude": 21.7499123}	Spacious dock with lounge seating.	3	1	180	18	t	2025-03-23 03:36:55.214364
7	Kamienna Bay Dock	{"Town": "Kamienna", "Latitude": 53.9809821, "Longitude": 21.7038123}	Tucked away for a quiet stay.	3	3	90	12	t	2025-03-23 03:36:55.214364
8	Sailor's Stop	{"Town": "Nowy Harbor", "Latitude": 54.0150123, "Longitude": 21.7638876}	Convenient for quick overnight stops.	3	4	110	14	t	2025-03-23 03:36:55.214364
9	Lighthouse Point	{"Town": "Laguna", "Latitude": 54.0321123, "Longitude": 21.7698876}	Iconic spot with lighthouse backdrop.	3	5	190	20	t	2025-03-23 03:36:55.214364
10	Green Hideaway	{"Town": "Hidden", "Latitude": 53.9691223, "Longitude": 21.7423371}	Secluded and peaceful dock area.	3	6	95	10	f	2025-03-23 03:36:55.214364
\.


--
-- TOC entry 5030 (class 0 OID 16501)
-- Dependencies: 232
-- Data for Name: Guides; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Guides" ("Id", "Title", "Content", "CreatedBy", "CreatedOn", "Location", "IsApproved") FROM stdin;
1	Gizycko - greatest place on earth	Gizycko - greatest place on earth. The end.	2	2025-03-23 02:34:21.592572	{"Town": "Gizycko", "Latitude": 54.0313983, "Longitude": 21.7679876}	t
2	Welcome to my Dock: Bogaczewo	Dock here and pay money, the best toilet outside and \n\tyou can take a shower when it is raining	3	2025-03-23 02:34:21.592572	{"Town": "Bogaczewo", "Latitude": 53.9629082, "Longitude": 21.7473430}	f
3	Sailing Through the Bay	The bay offers stunning views and calm waters...	4	2025-03-23 03:35:45.858643	{"Town": "Zloty Port", "Latitude": 54.2112981, "Longitude": 21.9341086}	t
4	Hidden Lagoon Entry	Watch for underwater rocks near the entrance...	6	2025-03-23 03:35:45.858643	{"Town": "Kamienna", "Latitude": 53.9818273, "Longitude": 21.7019281}	f
5	Docking Etiquette Tips	Be courteous when using shared docking zones...	5	2025-03-23 03:35:45.858643	{"Town": "Gizycko", "Latitude": 54.0328712, "Longitude": 21.7712387}	t
6	Local Seafood Spots	Visit these three lakefront seafood gems...	7	2025-03-23 03:35:45.858643	{"Town": "Gizycko", "Latitude": 54.0156738, "Longitude": 21.7639382}	t
7	Avoiding Shallow Zones	Always keep your eyes on the sonar here...	8	2025-03-23 03:35:45.858643	{"Town": "Bogaczewo", "Latitude": 53.9672239, "Longitude": 21.7428831}	f
8	Night Navigation Guide	Use the west-facing lighthouse as your landmark...	1	2025-03-23 03:35:45.858643	{"Town": "Bogaczewo", "Latitude": 53.9700123, "Longitude": 21.7404449}	t
9	Best Photo Spots	Get the perfect sunset shot from this dock...	2	2025-03-23 03:35:45.858643	{"Town": "Gizycko", "Latitude": 54.0310123, "Longitude": 21.7660099}	t
10	Supplies Checklist	Always carry these essentials before setting out...	3	2025-03-23 03:35:45.858643	{"Town": "Bogaczewo", "Latitude": 53.9651123, "Longitude": 21.7463377}	t
\.


--
-- TOC entry 5032 (class 0 OID 16517)
-- Dependencies: 234
-- Data for Name: Images; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Images" ("Id", "Url", "CreatedBy", "CreatedOn", "GuideId") FROM stdin;
1	\\\\Randomlink1	1	2025-03-23 02:50:23.841737	1
2	\\\\Randomlink2	2	2025-03-23 02:50:23.841737	2
3	\\\\image3.png	4	2025-03-23 03:38:49.079782	3
4	\\\\image4.png	5	2025-03-23 03:38:49.079782	4
5	\\\\image5.png	6	2025-03-23 03:38:49.079782	5
6	\\\\image6.png	7	2025-03-23 03:38:49.079782	6
7	\\\\image7.png	8	2025-03-23 03:38:49.079782	7
8	\\\\image8.png	9	2025-03-23 03:38:49.079782	8
9	\\\\image9.png	10	2025-03-23 03:38:49.079782	9
10	\\\\image10.png	1	2025-03-23 03:38:49.079782	10
\.


--
-- TOC entry 5038 (class 0 OID 16572)
-- Dependencies: 240
-- Data for Name: Notifications; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Notifications" ("Id", "CreatedBy", "Message", "CreatedOn") FROM stdin;
1	3	Where is my money for the spot?	2025-03-23 01:59:58.320548
2	4	You are getting your account deleted bro	2025-03-23 01:59:58.320548
3	3	Payment overdue. Please check your invoice.	2025-03-23 03:34:59.376024
4	5	New booking confirmed for next week.	2025-03-23 03:34:59.376024
5	2	Your port listing has been approved.	2025-03-23 03:34:59.376024
6	6	Reminder: Upload photos to your guide.	2025-03-23 03:34:59.376024
7	4	Weather alert: storm approaching.	2025-03-23 03:34:59.376024
8	1	Your password was changed successfully.	2025-03-23 03:34:59.376024
9	3	Guide has been reviewed. Check feedback.	2025-03-23 03:34:59.376024
10	2	New service request on your docking spot.	2025-03-23 03:34:59.376024
\.


--
-- TOC entry 5028 (class 0 OID 16489)
-- Dependencies: 230
-- Data for Name: PaymentMethods; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."PaymentMethods" ("Id", "Name") FROM stdin;
1	Online
2	On-site
\.


--
-- TOC entry 5020 (class 0 OID 16412)
-- Dependencies: 222
-- Data for Name: Ports; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Ports" ("Id", "CreatedOn", "Name", "Location", "Description", "OwnerId", "IsApproved") FROM stdin;
1	2025-03-23 02:40:57.901704	Bogaczewo	{"Town": "Bogaczewo", "Latitude": 53.9629082, "Longitude": 21.747343}	\N	3	t
2	2025-03-23 02:40:57.901704	Gizycko	{"Town": "Gizycko", "Latitude": 54.0313983, "Longitude": 21.7679876}	\N	3	t
3	2025-03-23 03:36:33.736456	Zloty Port	{"Town": "Zloty Port", "Latitude": 54.2112981, "Longitude": 21.9341086}	\N	3	t
4	2025-03-23 03:36:33.736456	Kamienna	{"Town": "Kamienna", "Latitude": 53.9818273, "Longitude": 21.7019281}	\N	3	t
5	2025-03-23 03:36:33.736456	Nowy Harbor	{"Town": "Nowy Harbor", "Latitude": 54.0451721, "Longitude": 21.7639123}	\N	3	t
6	2025-03-23 03:36:33.736456	Laguna	{"Town": "Laguna", "Latitude": 54.0312372, "Longitude": 21.7658123}	\N	3	t
7	2025-03-23 03:36:33.736456	Szczecinek Marina	{"Town": "Szczecinek", "Latitude": 54.0358732, "Longitude": 21.7643212}	\N	3	t
8	2025-03-23 03:36:33.736456	Hidden Dock	{"Town": "Hidden", "Latitude": 53.9672211, "Longitude": 21.7441221}	\N	3	t
\.


--
-- TOC entry 5036 (class 0 OID 16557)
-- Dependencies: 238
-- Data for Name: Reviews; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Reviews" ("Id", "CreatedBy", "Rating", "Comment", "CreatedOn", "PortId") FROM stdin;
1	1	5	Zbs	2025-03-23 03:09:03.551172	1
2	2	1	Gowno, nie polecam	2025-03-23 03:09:03.551172	2
3	3	4	Nice facilities, could be cleaner.	2025-03-23 03:40:18.451091	3
4	4	5	Amazing views and quiet nights.	2025-03-23 03:40:18.451091	4
5	5	3	Pretty average.	2025-03-23 03:40:18.451091	2
6	6	2	Too crowded during the day.	2025-03-23 03:40:18.451091	1
7	7	5	Absolutely perfect.	2025-03-23 03:40:18.451091	5
8	8	4	Easy access and friendly staff.	2025-03-23 03:40:18.451091	6
9	9	1	Wouldn’t dock here again.	2025-03-23 03:40:18.451091	3
10	10	3	Not bad, not great.	2025-03-23 03:40:18.451091	2
\.


--
-- TOC entry 5018 (class 0 OID 16398)
-- Dependencies: 220
-- Data for Name: Roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Roles" ("Id", "Name") FROM stdin;
1	User
2	Admin
3	Owner
\.


--
-- TOC entry 5024 (class 0 OID 16449)
-- Dependencies: 226
-- Data for Name: Services; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Services" ("Id", "Name", "Description", "Price", "PortId", "DockingSpotId", "IsAvailable", "CreatedOn") FROM stdin;
1	Water Refill	Fresh water refill service for your boat.	15.00	1	1	t	2025-03-23 03:12:08.449666
2	Electricity Hookup	Provides 220V shore power connection.	25.00	1	2	t	2025-03-23 03:12:08.449666
3	Boat Cleaning	Complete exterior cleaning service.	100.00	2	3	t	2025-03-23 03:12:08.449666
4	Laundry	Drop-off laundry service available at the port office.	35.00	2	\N	t	2025-03-23 03:12:08.449666
5	Shower Access	Private shower cabin access near the docking area.	10.00	2	4	f	2025-03-23 03:12:08.449666
6	Local Tour	Guided boat tour around the lake and nearby landmarks.	60.00	1	\N	t	2025-03-23 03:12:08.449666
7	Fuel Station	Diesel and petrol available for refueling.	110.00	3	5	t	2025-03-23 03:40:56.117492
8	Kayak Rental	Hourly kayak rentals available.	45.00	2	3	t	2025-03-23 03:40:56.117492
9	Wi-Fi Access	Premium marina Wi-Fi pass.	15.00	1	2	f	2025-03-23 03:40:56.117492
10	Snack Bar	Get snacks and drinks near the dock.	20.00	4	6	t	2025-03-23 03:40:56.117492
\.


--
-- TOC entry 5016 (class 0 OID 16388)
-- Dependencies: 218
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "CreatedOn", "Name", "Surname", "Email", "PhoneNumber", "Password", "RoleId") FROM stdin;
1	2025-03-23 01:30:40.921318	Denzel	Washington	thegoat@gmail.com	+1(555) 555-1234	hashedwillchangelater1	1
2	2025-03-23 01:30:40.921318	Andrzej	Duda	dialog@prezydent.pl	+48 777 777 777	hashedwillchangelater2	1
3	2025-03-23 01:30:40.921318	Dock	Owner	paymemoney@gmail.com	+48 123 456 789	hashedwillchangelater3	3
4	2025-03-23 01:30:40.921318	Mr	Admin	willdeleteyouraccount@gmail.com	+48 987 654 321	hashedwillchangelater4	2
5	2025-03-23 03:34:27.920004	Wendy	Nguyen	portwind69@example.com	555-202-5832	hashedpass4	2
6	2025-03-23 03:34:27.920004	Miguel	Lopez	ml_crew27@example.com	555-487-2394	hashedpass5	1
7	2025-03-23 03:34:27.920004	Lea	Kowalski	leasails@email.com	555-990-1532	hashedpass6	3
8	2025-03-23 03:34:27.920004	Jonas	Eriksson	jonas.swede@example.com	555-420-1337	hashedpass7	1
9	2025-03-23 03:34:27.920004	Elena	Petrova	e.petrova@yachts.ru	555-610-8373	hashedpass8	2
10	2025-03-23 03:34:27.920004	Isaac	Brown	isaac.b@harbors.com	555-334-6788	hashedpass9	3
\.


--
-- TOC entry 5056 (class 0 OID 0)
-- Dependencies: 227
-- Name: bookings_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.bookings_id_seq', 1, false);


--
-- TOC entry 5057 (class 0 OID 0)
-- Dependencies: 235
-- Name: comments_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.comments_id_seq', 10, true);


--
-- TOC entry 5058 (class 0 OID 0)
-- Dependencies: 223
-- Name: dockingspots_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dockingspots_id_seq', 10, true);


--
-- TOC entry 5059 (class 0 OID 0)
-- Dependencies: 231
-- Name: guides_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.guides_id_seq', 10, true);


--
-- TOC entry 5060 (class 0 OID 0)
-- Dependencies: 233
-- Name: images_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.images_id_seq', 10, true);


--
-- TOC entry 5061 (class 0 OID 0)
-- Dependencies: 239
-- Name: notifications_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.notifications_id_seq', 10, true);


--
-- TOC entry 5062 (class 0 OID 0)
-- Dependencies: 229
-- Name: paymentmethods_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.paymentmethods_id_seq', 2, true);


--
-- TOC entry 5063 (class 0 OID 0)
-- Dependencies: 221
-- Name: port_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.port_id_seq', 8, true);


--
-- TOC entry 5064 (class 0 OID 0)
-- Dependencies: 237
-- Name: reviews_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reviews_id_seq', 10, true);


--
-- TOC entry 5065 (class 0 OID 0)
-- Dependencies: 219
-- Name: roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.roles_id_seq', 3, true);


--
-- TOC entry 5066 (class 0 OID 0)
-- Dependencies: 225
-- Name: services_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.services_id_seq', 10, true);


--
-- TOC entry 5067 (class 0 OID 0)
-- Dependencies: 217
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 10, true);


--
-- TOC entry 4840 (class 2606 OID 16476)
-- Name: Bookings Bookings_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Bookings"
    ADD CONSTRAINT "Bookings_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4848 (class 2606 OID 16545)
-- Name: Comments Comments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Comments"
    ADD CONSTRAINT "Comments_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4836 (class 2606 OID 16437)
-- Name: DockingSpots DockingSpots_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DockingSpots"
    ADD CONSTRAINT "DockingSpots_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4844 (class 2606 OID 16510)
-- Name: Guides Guides_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Guides"
    ADD CONSTRAINT "Guides_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4846 (class 2606 OID 16525)
-- Name: Images Images_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Images"
    ADD CONSTRAINT "Images_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4852 (class 2606 OID 16580)
-- Name: Notifications Notifications_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "Notifications_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4842 (class 2606 OID 16494)
-- Name: PaymentMethods PaymentMethods_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PaymentMethods"
    ADD CONSTRAINT "PaymentMethods_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4834 (class 2606 OID 16421)
-- Name: Ports Ports_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Ports"
    ADD CONSTRAINT "Ports_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4850 (class 2606 OID 16565)
-- Name: Reviews Reviews_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reviews"
    ADD CONSTRAINT "Reviews_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4830 (class 2606 OID 16405)
-- Name: Roles Roles_Name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "Roles_Name_key" UNIQUE ("Name");


--
-- TOC entry 4832 (class 2606 OID 16403)
-- Name: Roles Roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "Roles_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4838 (class 2606 OID 16457)
-- Name: Services Services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "Services_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4826 (class 2606 OID 16396)
-- Name: Users Users_Email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_Email_key" UNIQUE ("Email");


--
-- TOC entry 4828 (class 2606 OID 16394)
-- Name: Users Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 4824 (class 1259 OID 16587)
-- Name: IDX_Users_Email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IDX_Users_Email" ON public."Users" USING btree ("Email");


--
-- TOC entry 4862 (class 2606 OID 16511)
-- Name: Guides FK_AuthorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Guides"
    ADD CONSTRAINT "FK_AuthorId" FOREIGN KEY ("CreatedBy") REFERENCES public."Users"("Id");


--
-- TOC entry 4863 (class 2606 OID 16526)
-- Name: Images FK_CreatedBy; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Images"
    ADD CONSTRAINT "FK_CreatedBy" FOREIGN KEY ("CreatedBy") REFERENCES public."Users"("Id");


--
-- TOC entry 4865 (class 2606 OID 16546)
-- Name: Comments FK_CreatedBy; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Comments"
    ADD CONSTRAINT "FK_CreatedBy" FOREIGN KEY ("CreatedBy") REFERENCES public."Users"("Id");


--
-- TOC entry 4867 (class 2606 OID 16566)
-- Name: Reviews FK_CreatedBy; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reviews"
    ADD CONSTRAINT "FK_CreatedBy" FOREIGN KEY ("CreatedBy") REFERENCES public."Users"("Id");


--
-- TOC entry 4869 (class 2606 OID 16581)
-- Name: Notifications FK_CreatedBy; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "FK_CreatedBy" FOREIGN KEY ("CreatedBy") REFERENCES public."Users"("Id");


--
-- TOC entry 4857 (class 2606 OID 16463)
-- Name: Services FK_DockingSpotId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "FK_DockingSpotId" FOREIGN KEY ("DockingSpotId") REFERENCES public."DockingSpots"("Id");


--
-- TOC entry 4859 (class 2606 OID 16482)
-- Name: Bookings FK_DockingSpotId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Bookings"
    ADD CONSTRAINT "FK_DockingSpotId" FOREIGN KEY ("DockingSpotId") REFERENCES public."DockingSpots"("Id");


--
-- TOC entry 4864 (class 2606 OID 16531)
-- Name: Images FK_GuideId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Images"
    ADD CONSTRAINT "FK_GuideId" FOREIGN KEY ("GuideId") REFERENCES public."Guides"("Id");


--
-- TOC entry 4866 (class 2606 OID 16551)
-- Name: Comments FK_GuideId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Comments"
    ADD CONSTRAINT "FK_GuideId" FOREIGN KEY ("GuideId") REFERENCES public."Guides"("Id");


--
-- TOC entry 4854 (class 2606 OID 16422)
-- Name: Ports FK_OwnerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Ports"
    ADD CONSTRAINT "FK_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."Users"("Id");


--
-- TOC entry 4855 (class 2606 OID 16438)
-- Name: DockingSpots FK_OwnerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DockingSpots"
    ADD CONSTRAINT "FK_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."Users"("Id");


--
-- TOC entry 4860 (class 2606 OID 16495)
-- Name: Bookings FK_PaymentMethodId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Bookings"
    ADD CONSTRAINT "FK_PaymentMethodId" FOREIGN KEY ("PaymentMethodId") REFERENCES public."PaymentMethods"("Id");


--
-- TOC entry 4856 (class 2606 OID 16443)
-- Name: DockingSpots FK_PortId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."DockingSpots"
    ADD CONSTRAINT "FK_PortId" FOREIGN KEY ("PortId") REFERENCES public."Ports"("Id");


--
-- TOC entry 4858 (class 2606 OID 16458)
-- Name: Services FK_PortId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Services"
    ADD CONSTRAINT "FK_PortId" FOREIGN KEY ("PortId") REFERENCES public."Ports"("Id");


--
-- TOC entry 4868 (class 2606 OID 16588)
-- Name: Reviews FK_PortId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Reviews"
    ADD CONSTRAINT "FK_PortId" FOREIGN KEY ("PortId") REFERENCES public."Ports"("Id");


--
-- TOC entry 4853 (class 2606 OID 16406)
-- Name: Users FK_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "FK_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id");


--
-- TOC entry 4861 (class 2606 OID 16477)
-- Name: Bookings FK_SailorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Bookings"
    ADD CONSTRAINT "FK_SailorId" FOREIGN KEY ("SailorId") REFERENCES public."Users"("Id");


-- Completed on 2025-03-23 03:57:20

--
-- PostgreSQL database dump complete
--

